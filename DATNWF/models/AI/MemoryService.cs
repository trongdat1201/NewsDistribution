using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATNWF.Models.AI
{
    /// <summary>
    /// Manages conversation memory with token-aware sliding window,
    /// summarization, and message pruning.
    /// </summary>
    public class MemoryService
    {
        private const double CharsPerToken = 2.5;
        
        private readonly List<ConversationMessage> _messages = new List<ConversationMessage>();

        public int MaxMessages => AiConfig.MaxHistoryMessages;
        public int MaxTokensPerMessage => AiConfig.MaxTokensPerMessage;

        public IReadOnlyList<ConversationMessage> Messages => _messages.AsReadOnly();
        public int Count => _messages.Count;
        public int EstimatedTokens => EstimateTotalTokens();

        /// <summary>
        /// Add a message. Triggers pruning when MaxMessages is exceeded.
        /// </summary>
        public void AddMessage(string role, string content)
        {
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(content))
                return;

            _messages.Add(new ConversationMessage { Role = role.ToLowerInvariant(), Content = content });

            if (_messages.Count > MaxMessages)
            {
                PruneOldest();
            }
        }

        /// <summary>
        /// Add an assistant (model) message and a user (tool result) message atomically.
        /// </summary>
        public void AddToolTurn(string modelContent, string toolResultContent)
        {
            if (!string.IsNullOrEmpty(modelContent))
                _messages.Add(new ConversationMessage { Role = "model", Content = modelContent });
            if (!string.IsNullOrEmpty(toolResultContent))
                _messages.Add(new ConversationMessage { Role = "user", Content = toolResultContent });

            if (_messages.Count > MaxMessages)
            {
                PruneOldest();
            }
        }

        /// <summary>
        /// Replace the last model message with a summarized version.
        /// The summarization should be done externally using an AI provider.
        /// </summary>
        /// <param name="originalContent">The original message content</param>
        /// <param name="summarizedContent">The AI-generated summary</param>
        /// <returns>True if replacement was successful, false if content not found</returns>
        public bool ReplaceWithSummary(string originalContent, string summarizedContent)
        {
            for (int i = _messages.Count - 1; i >= 0; i--)
            {
                if (_messages[i].Role == "model" && _messages[i].Content == originalContent)
                {
                    _messages[i] = new ConversationMessage
                    {
                        Role = "model",
                        Content = $"[TÓM TẮT từ {EstimateTokens(originalContent)} tokens]: {summarizedContent}"
                    };
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the conversation formatted as plain text for external summarization.
        /// </summary>
        public string GetTranscript()
        {
            var sb = new StringBuilder();
            foreach (var msg in _messages)
            {
                sb.AppendLine($"{msg.Role.ToUpper()}: {msg.Content}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns recent messages formatted for context injection.
        /// </summary>
        /// <param name="maxMessages">Maximum number of messages to return</param>
        public string GetRecentContext(int maxMessages = 10)
        {
            var startIdx = Math.Max(0, _messages.Count - maxMessages);
            var recent = _messages.Skip(startIdx).ToList();
            var sb = new StringBuilder();
            foreach (var msg in recent)
            {
                sb.AppendLine($"{msg.Role}: {TruncateForContext(msg.Content)}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Estimates total tokens in conversation using word-based approximation.
        /// More accurate than character-based for Vietnamese text.
        /// </summary>
        public int EstimateTotalTokens()
        {
            return _messages.Sum(m => EstimateTokens(m.Content));
        }

        /// <summary>
        /// Estimates tokens in text using word-based approximation.
        /// For Vietnamese: ~2.5 chars per token is more accurate than 4.
        /// </summary>
        public static int EstimateTokens(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            int wordCount = text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            int charCount = text.Count(c => !char.IsWhiteSpace(c));

            return Math.Max(wordCount, (int)Math.Ceiling(charCount / 2.5));
        }

        /// <summary>
        /// Truncates text for context if it exceeds max tokens.
        /// Uses 2.5 chars per token for Vietnamese text.
        /// </summary>
        private static string TruncateForContext(string text, int maxTokens = 500)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            int estimated = EstimateTokens(text);
            if (estimated <= maxTokens)
                return text;

            int targetChars = (int)(maxTokens * CharsPerToken);
            if (targetChars >= text.Length)
                return text;

            return text.Substring(0, targetChars) + "...[đã cắt]";
        }

        /// <summary>
        /// Truncates very long messages in the history to save context window.
        /// </summary>
        public void TruncateLongMessages(int maxTokensPerMessage = 0)
        {
            if (maxTokensPerMessage <= 0)
                maxTokensPerMessage = MaxTokensPerMessage;

            for (int i = 0; i < _messages.Count; i++)
            {
                int estimated = EstimateTokens(_messages[i].Content);
                if (estimated > maxTokensPerMessage)
                {
                    int targetChars = (int)(maxTokensPerMessage * CharsPerToken);
                    int actualChars = Math.Min(targetChars, _messages[i].Content.Length);
                    _messages[i] = new ConversationMessage
                    {
                        Role = _messages[i].Role,
                        Content = _messages[i].Content.Substring(0, actualChars) + "...[đã cắt ngắn]"
                    };
                }
            }
        }

        /// <summary>
        /// Removes the oldest non-system message pair while preserving system messages.
        /// </summary>
        private void PruneOldest()
        {
            if (_messages.Count <= 2)
                return;

            int removed = 0;
            int target = 2;

            for (int i = 0; i < _messages.Count && removed < target; i++)
            {
                string role = _messages[i].Role.ToLowerInvariant();
                if (role != "system")
                {
                    _messages.RemoveAt(i);
                    i--;
                    removed++;
                }
            }
        }

        /// <summary>
        /// Clear all history.
        /// </summary>
        public void Clear()
        {
            _messages.Clear();
        }

        /// <summary>
        /// Compact the history: truncate long messages first, then prune oldest if still over limit.
        /// </summary>
        public void Compact()
        {
            TruncateLongMessages();
            while (_messages.Count > MaxMessages)
            {
                PruneOldest();
            }
        }

        /// <summary>
        /// Build a ConversationHistory for the AI provider from current messages.
        /// </summary>
        public ConversationHistory ToConversationHistory()
        {
            return new ConversationHistory { Messages = _messages.ToList() };
        }

        /// <summary>
        /// Check if memory is approaching limits and needs compaction.
        /// </summary>
        public bool NeedsCompaction => _messages.Count > MaxMessages * 0.8 || EstimatedTokens > MaxTokensPerMessage * 10;

        /// <summary>
        /// Get summary statistics for debugging/monitoring.
        /// </summary>
        public string GetStats()
        {
            return $"Messages: {Count}, Estimated Tokens: {EstimatedTokens}, Needs Compaction: {NeedsCompaction}";
        }
    }
}

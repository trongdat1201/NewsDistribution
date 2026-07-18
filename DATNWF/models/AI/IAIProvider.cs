using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DATNWF.Models.AI
{
    public interface IAIProvider
    {
        string ProviderName { get; }
        bool IsAvailable { get; }
        /// <summary>
        /// Optional: subscribe để nhận log structured thay vì dùng Console.WriteLine.
        /// Args: (level: "info"|"warn"|"error", message).
        /// </summary>
        event Action<string, string> OnLog;
        Task<AIResponse> SendMessageAsync(string userMessage, ConversationHistory history, System.Threading.CancellationToken cancellationToken = default);
        Task<AIResponse> SendMessageWithSystemAsync(string userMessage, ConversationHistory history, string systemPrompt, System.Threading.CancellationToken cancellationToken = default);
        Task<bool> HealthCheckAsync(System.Threading.CancellationToken cancellationToken = default);
    }

    public class AIResponse
    {
        public bool Success { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public long LatencyMs { get; set; }
        public bool WantsToolCall { get; set; }
        public ToolCallRequest ToolCall { get; set; }
    }

    public class ConversationMessage
    {
        public string Role { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    public class ConversationHistory
    {
        public System.Collections.Generic.List<ConversationMessage> Messages { get; set; } = new System.Collections.Generic.List<ConversationMessage>();
    }
}

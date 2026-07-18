using System;

namespace DATNWF.Models.AI
{
    /// <summary>
    /// Centralized AI configuration — reads from App.config and environment variables.
    /// Singleton via Lazy&lt;T&gt; so each setting is resolved at most once per process.
    /// </summary>
    public static class AiConfig
    {
        #region Lazy caches — resolved exactly once

        private static readonly Lazy<string> _geminiApiKey = new Lazy<string>(
            () => ReadEnvOrConfig("GEMINI_API_KEY", "GeminiApiKey", string.Empty));

        private static readonly Lazy<string> _geminiModel = new Lazy<string>(
            () => ReadEnvOrConfig("GEMINI_MODEL", "GeminiModel", "gemini-3.5-flash"));

        private static readonly Lazy<string> _geminiApiUrl = new Lazy<string>(
            () => GetAppSetting("GeminiApiUrl", "https://generativelanguage.googleapis.com/v1beta/models"));

        private static readonly Lazy<string> _ollamaBaseUrl = new Lazy<string>(
            () => GetAppSetting("OllamaBaseUrl", "http://localhost:11434"));

        private static readonly Lazy<string> _ollamaModel = new Lazy<string>(
            () => ReadEnvOrConfig("OLLAMA_MODEL", "OllamaModel", "qwen3.6"));

        private static readonly Lazy<string> _embeddingModel = new Lazy<string>(
            () => GetAppSetting("EmbeddingModel", "nomic-embed-text"));

        private static readonly Lazy<bool> _useOllamaEmbeddings = new Lazy<bool>(
            () => bool.TryParse(GetAppSetting("UseOllamaEmbeddings", "true"), out var v) && v);

        private static readonly Lazy<int> _ollamaTimeoutSeconds = new Lazy<int>(
            () => int.TryParse(GetAppSetting("OllamaTimeoutSeconds", "60"), out var v) ? v : 60);

        private static readonly Lazy<string> _claudeApiKey = new Lazy<string>(
            () => ReadEnvOrConfig("CLAUDE_API_KEY", "ClaudeApiKey", string.Empty));

        private static readonly Lazy<string> _claudeBaseUrl = new Lazy<string>(
            () => GetAppSetting("ClaudeBaseUrl", "https://modelapi.vn"));

        private static readonly Lazy<string> _claudeModel = new Lazy<string>(
            () => ReadEnvOrConfig("CLAUDE_MODEL", "ClaudeModel", "claude-sonnet-4-6"));

        private static readonly Lazy<AIProviderMode> _providerMode = new Lazy<AIProviderMode>(
            () =>
            {
                var modeStr = GetAppSetting("AIProviderMode", "2");
                return Enum.TryParse<AIProviderMode>(modeStr, out var m) ? m : AIProviderMode.Hybrid;
            });

        private static readonly Lazy<int> _maxHistoryMessages = new Lazy<int>(
            () => int.TryParse(GetAppSetting("AIMaxHistoryMessages", "50"), out var v) ? v : 50);

        private static readonly Lazy<int> _maxTokensPerMessage = new Lazy<int>(
            () => int.TryParse(GetAppSetting("AIMaxTokensPerMessage", "500"), out var v) ? v : 500);

        private static readonly Lazy<bool> _ragEnabled = new Lazy<bool>(
            () => bool.TryParse(GetAppSetting("AIRagEnabled", "true"), out var v) && v);

        private static readonly Lazy<int> _maxRagContextChars = new Lazy<int>(
            () => int.TryParse(GetAppSetting("AIMaxRagContextChars", "2000"), out var v) ? v : 2000);

        #endregion

        #region Gemini

        public static string GeminiApiKey => _geminiApiKey.Value;

        public static string GeminiModel => _geminiModel.Value;

        public static string GeminiApiUrl => _geminiApiUrl.Value;

        public static bool IsGeminiConfigured => !string.IsNullOrWhiteSpace(GeminiApiKey);

        #endregion

        #region Ollama

        public static string OllamaBaseUrl => _ollamaBaseUrl.Value;

        public static string OllamaModel => _ollamaModel.Value;

        /// <summary>
        /// Embedding model dùng cho RAG (default: nomic-embed-text).
        /// </summary>
        public static string EmbeddingModel => _embeddingModel.Value;

        /// <summary>
        /// Có dùng Ollama cho embeddings không.
        /// </summary>
        public static bool UseOllamaEmbeddings => _useOllamaEmbeddings.Value;

        public static int OllamaTimeoutSeconds => _ollamaTimeoutSeconds.Value;

        #endregion

        #region Claude

        public static string ClaudeApiKey => _claudeApiKey.Value;

        public static string ClaudeBaseUrl => _claudeBaseUrl.Value;

        public static string ClaudeModel => _claudeModel.Value;

        public static bool IsClaudeConfigured => !string.IsNullOrWhiteSpace(ClaudeApiKey);

        #endregion

        #region General

        public static AIProviderMode ProviderMode => _providerMode.Value;

        /// <summary>
        /// Maximum conversation messages to keep before summarizing/pruning.
        /// </summary>
        public static int MaxHistoryMessages => _maxHistoryMessages.Value;

        /// <summary>
        /// Approximate max tokens to keep per message after summarization.
        /// </summary>
        public static int MaxTokensPerMessage => _maxTokensPerMessage.Value;

        /// <summary>
        /// Whether to prepend RAG context to every user message.
        /// </summary>
        public static bool RagEnabled => _ragEnabled.Value;

        /// <summary>
        /// Max RAG context characters to inject into the prompt.
        /// </summary>
        public static int MaxRagContextChars => _maxRagContextChars.Value;

        #endregion

        #region Helpers

        private static string GetAppSetting(string key, string defaultValue)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key] ?? defaultValue;
        }

        /// <summary>
        /// Returns <paramref name="fallback"/> 
        /// </summary>
        private static string ReadEnvOrConfig(string envKey, string appKey, string fallback)
        {
            var env = Environment.GetEnvironmentVariable(envKey, EnvironmentVariableTarget.User)
                ?? Environment.GetEnvironmentVariable(envKey, EnvironmentVariableTarget.Machine)
                ?? Environment.GetEnvironmentVariable(envKey, EnvironmentVariableTarget.Process);
            if (!string.IsNullOrWhiteSpace(env))
                return env;
            return GetAppSetting(appKey, fallback);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DATNWF.Models.AI
{
    public class GeminiProvider : BaseAIProvider, IAIProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _modelName;
        private readonly string _baseUrl;

        public string ProviderName => "Gemini";

        public bool IsAvailable => !string.IsNullOrWhiteSpace(_apiKey);

        public event Action<string, string> OnLog;

        public GeminiProvider()
        {
            _apiKey = AiConfig.GeminiApiKey;
            _modelName = AiConfig.GeminiModel;
            _baseUrl = AiConfig.GeminiApiUrl;
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
        }

        public async Task<AIResponse> SendMessageAsync(
            string userMessage,
            ConversationHistory history,
            CancellationToken cancellationToken = default)
        {
            return await SendMessageInternalAsync(userMessage, history, null, cancellationToken);
        }

        public async Task<AIResponse> SendMessageWithSystemAsync(
            string userMessage,
            ConversationHistory history,
            string systemPrompt,
            CancellationToken cancellationToken = default)
        {
            return await SendMessageInternalAsync(userMessage, history, systemPrompt, cancellationToken);
        }

        private async Task<AIResponse> SendMessageInternalAsync(
            string userMessage,
            ConversationHistory history,
            string systemPrompt,
            CancellationToken cancellationToken)
        {
            if (!IsAvailable)
                return new AIResponse { Success = false, Error = "Gemini API key not configured.", Provider = ProviderName };

            var sw = Stopwatch.StartNew();

            try
            {
                var endpoint = $"{_baseUrl.TrimEnd('/')}/{_modelName}:generateContent?key={_apiKey}";

                var messages = new List<object>();

                foreach (var msg in history.Messages)
                {
                    messages.Add(new { role = msg.Role == "model" ? "model" : "user", parts = new[] { new { text = msg.Content } } });
                }
                messages.Add(new { role = "user", parts = new[] { new { text = userMessage } } });

                object requestBody = string.IsNullOrWhiteSpace(systemPrompt)
                    ? (object)new { contents = messages }
                    : new
                    {
                        contents = messages,
                        systemInstruction = new { parts = new[] { new { text = systemPrompt } } }
                    };

                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);
                var responseText = await response.Content.ReadAsStringAsync();
                sw.Stop();

                if (!response.IsSuccessStatusCode)
                {
                    return new AIResponse
                    {
                        Success = false,
                        Error = $"Gemini API error {response.StatusCode}: {responseText}",
                        Provider = ProviderName,
                        LatencyMs = sw.ElapsedMilliseconds
                    };
                }

                dynamic json = JsonConvert.DeserializeObject(responseText);
                string reply = json?.candidates?[0]?.content?.parts?[0]?.text ?? "";

                var aiResponse = new AIResponse
                {
                    Success = true,
                    Text = reply?.Trim() ?? "",
                    Provider = ProviderName,
                    LatencyMs = sw.ElapsedMilliseconds
                };

                aiResponse.ToolCall = TryParseToolCall(reply);
                if (aiResponse.ToolCall != null)
                    aiResponse.WantsToolCall = true;

                return aiResponse;
            }
            catch (OperationCanceledException)
            {
                return new AIResponse { Success = false, Error = "Request cancelled.", Provider = ProviderName, LatencyMs = sw.ElapsedMilliseconds };
            }
            catch (Exception ex)
            {
                sw.Stop();
                return new AIResponse { Success = false, Error = ex.Message, Provider = ProviderName, LatencyMs = sw.ElapsedMilliseconds };
            }
        }

        public async Task<bool> HealthCheckAsync(CancellationToken cancellationToken = default)
        {
            if (!IsAvailable) return false;
            try
            {
                var probe = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
                var url = $"{_baseUrl.TrimEnd('/')}/{_modelName}?key={_apiKey}";
                var resp = await probe.GetAsync(url, cancellationToken);
                return resp.IsSuccessStatusCode || (int)resp.StatusCode == 400;
            }
            catch
            {
                return false;
            }
        }
    }
}

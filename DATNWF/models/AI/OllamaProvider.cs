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
    public class OllamaProvider : BaseAIProvider, IAIProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _modelName;
        private readonly int _timeoutSeconds;

        public string ProviderName => "Ollama";

        public bool IsAvailable { get; private set; }

        public event Action<string, string> OnLog;

        public OllamaProvider()
        {
            _modelName = AiConfig.OllamaModel;
            _timeoutSeconds = AiConfig.OllamaTimeoutSeconds;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(AiConfig.OllamaBaseUrl),
                Timeout = TimeSpan.FromSeconds(_timeoutSeconds)
            };
            IsAvailable = false;

            // Auto-probe ngay trong constructor (fire-and-forget).
            // Probe chỉ tốn ~5ms nếu service sẵn sàng, timeout nếu không.
            _ = ProbeAvailabilityAsync();
        }

        private async Task ProbeAvailabilityAsync()
        {
            try
            {
                using var probe = new HttpClient { Timeout = TimeSpan.FromSeconds(3) };
                var resp = await probe.GetAsync($"{AiConfig.OllamaBaseUrl}/api/tags");
                IsAvailable = resp.IsSuccessStatusCode;
            }
            catch
            {
                IsAvailable = false;
            }
        }

        public async Task<AIResponse> SendMessageAsync(
            string userMessage,
            ConversationHistory history,
            CancellationToken cancellationToken = default)
        {
            return await SendInternalAsync(userMessage, history, null, cancellationToken);
        }

        public async Task<AIResponse> SendMessageWithSystemAsync(
            string userMessage,
            ConversationHistory history,
            string systemPrompt,
            CancellationToken cancellationToken = default)
        {
            return await SendInternalAsync(userMessage, history, systemPrompt, cancellationToken);
        }

        private async Task<AIResponse> SendInternalAsync(
            string userMessage,
            ConversationHistory history,
            string systemPrompt,
            CancellationToken cancellationToken)
        {
            if (!IsAvailable)
                return new AIResponse { Success = false, Error = "Ollama service unavailable.", Provider = ProviderName };

            var sw = Stopwatch.StartNew();

            try
            {
                var messages = new List<object>();

                if (!string.IsNullOrWhiteSpace(systemPrompt))
                    messages.Add(new { role = "system", content = systemPrompt });

                foreach (var msg in history.Messages)
                {
                    messages.Add(new { role = msg.Role == "model" ? "assistant" : "user", content = msg.Content });
                }
                messages.Add(new { role = "user", content = userMessage });

                var requestBody = new
                {
                    model = _modelName,
                    messages = messages,
                    stream = false,
                    options = new { temperature = 0.7 }
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("/api/chat", content, cancellationToken);
                var responseText = await response.Content.ReadAsStringAsync();
                sw.Stop();

                if (!response.IsSuccessStatusCode)
                {
                    return new AIResponse
                    {
                        Success = false,
                        Error = $"Ollama error {response.StatusCode}: {responseText}",
                        Provider = ProviderName,
                        LatencyMs = sw.ElapsedMilliseconds
                    };
                }

                dynamic json = JsonConvert.DeserializeObject(responseText);
                string reply = json?.message?.content ?? "";

                var aiResponse = new AIResponse
                {
                    Success = true,
                    Text = reply.Trim(),
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
            try
            {
                var response = await _httpClient.GetAsync("/api/tags", cancellationToken);
                IsAvailable = response.IsSuccessStatusCode;
                return IsAvailable;
            }
            catch { IsAvailable = false; return false; }
        }
    }
}

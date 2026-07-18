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
    public class ClaudeProvider : BaseAIProvider, IAIProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _modelName;
        private readonly string _baseUrl;
        private const int MaxTokens = 8192;
        private const string ApiVersion = "2023-06-01";

        public string ProviderName => "Claude";

        public bool IsAvailable => !string.IsNullOrWhiteSpace(_apiKey);

        public event Action<string, string> OnLog;

        public ClaudeProvider()
        {
            _apiKey = AiConfig.ClaudeApiKey;
            _modelName = AiConfig.ClaudeModel;
            _baseUrl = AiConfig.ClaudeBaseUrl;
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(120) };
        }

        public async Task<AIResponse> SendMessageAsync(
            string userMessage,
            ConversationHistory history,
            CancellationToken cancellationToken = default)
        {
            return await SendMessageInternalAsync(userMessage, history, string.Empty, cancellationToken);
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
                return new AIResponse { Success = false, Error = "Claude API key not configured. Set CLAUDE_API_KEY environment variable.", Provider = ProviderName };

            var sw = Stopwatch.StartNew();

            try
            {
                var endpoint = $"{_baseUrl.TrimEnd('/')}/v1/messages";

                var messages = new List<object>();
                foreach (var msg in history.Messages)
                {
                    messages.Add(new { role = msg.Role == "model" ? "assistant" : "user", content = msg.Content });
                }
                messages.Add(new { role = "user", content = userMessage });

                var requestBody = new Dictionary<string, object>
                {
                    { "model", _modelName },
                    { "max_tokens", MaxTokens },
                    { "messages", messages }
                };

                if (!string.IsNullOrWhiteSpace(systemPrompt))
                {
                    requestBody["system"] = systemPrompt;
                }

                var jsonBody = JsonConvert.SerializeObject(requestBody, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
                request.Headers.Add("Authorization", $"Bearer {_apiKey}");
                request.Headers.Add("anthropic-version", ApiVersion);
                request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request, cancellationToken);
                var responseText = await response.Content.ReadAsStringAsync();
                sw.Stop();

                if (!response.IsSuccessStatusCode)
                {
                    return new AIResponse
                    {
                        Success = false,
                        Error = $"Claude API error {response.StatusCode}: {responseText}",
                        Provider = ProviderName,
                        LatencyMs = sw.ElapsedMilliseconds
                    };
                }

                dynamic json = JsonConvert.DeserializeObject(responseText);

                string reply = string.Empty;
                if (json?.content != null)
                {
                    foreach (var block in json.content)
                    {
                        if (block?.type?.ToString() == "text" && block?.text != null)
                        {
                            reply = block.text.ToString();
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(reply))
                    reply = (json?.content != null && json.content.Count > 0) ? json.content[0].text?.ToString() ?? string.Empty : string.Empty;

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
            if (!IsAvailable) return false;
            try
            {
                var probe = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
                var resp = await probe.GetAsync(_baseUrl, cancellationToken);
                return resp.IsSuccessStatusCode || (int)resp.StatusCode < 500;
            }
            catch
            {
                return false;
            }
        }
    }
}

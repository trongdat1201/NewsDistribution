using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DATNWF.Models.AI
{
    /// <summary>
    /// Embeddings service sử dụng Ollama local.
    /// Nhanh, miễn phí, không gửi data ra ngoài.
    /// Có caching để tránh re-embed text giống nhau.
    /// </summary>
    public class OllamaEmbeddings : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _model;
        private readonly ConcurrentDictionary<string, float[]> _cache;
        private bool _disposed;

        public string ProviderName => "Ollama-Embeddings";

        public OllamaEmbeddings()
        {
            _baseUrl = AiConfig.OllamaBaseUrl;
            _model = AiConfig.EmbeddingModel;
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
            _cache = new ConcurrentDictionary<string, float[]>();
        }

        /// <summary>
        /// Tạo embedding vector từ một đoạn text.
        /// Có caching để tránh re-embed text giống nhau.
        /// </summary>
        public async Task<float[]> EmbedAsync(string text, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(text))
                return Array.Empty<float>();

            // Check cache first
            if (_cache.TryGetValue(text, out var cached))
                return cached;

            try
            {
                var requestBody = new
                {
                    model = _model,
                    prompt = text
                };

                var jsonBody = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/embeddings", content, ct);
                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    LogError($"Embed API error: {response.StatusCode} - {responseText}");
                    return Array.Empty<float>();
                }

                dynamic json = JsonConvert.DeserializeObject(responseText);
                if (json?.embedding == null)
                {
                    LogError("No embedding returned from Ollama");
                    return Array.Empty<float>();
                }

                var embedding = new List<float>();
                foreach (var val in json.embedding)
                {
                    embedding.Add((float)val);
                }

                var result = embedding.ToArray();

                // Cache the result
                _cache.TryAdd(text, result);

                return result;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LogError($"Embed failed: {ex.Message}");
                return Array.Empty<float>();
            }
        }

        /// <summary>
        /// Tạo embeddings cho nhiều texts cùng lúc (batch).
        /// </summary>
        public async Task<List<float[]>> EmbedBatchAsync(IEnumerable<string> texts, CancellationToken ct = default)
        {
            var results = new List<float[]>();

            foreach (var text in texts)
            {
                var embedding = await EmbedAsync(text, ct);
                results.Add(embedding);
            }

            return results;
        }

        /// <summary>
        /// Tính cosine similarity giữa 2 vectors.
        /// </summary>
        public static float CosineSimilarity(float[] a, float[] b)
        {
            if (a == null || b == null || a.Length != b.Length || a.Length == 0)
                return 0;

            float dotProduct = 0;
            float normA = 0;
            float normB = 0;

            for (int i = 0; i < a.Length; i++)
            {
                dotProduct += a[i] * b[i];
                normA += a[i] * a[i];
                normB += b[i] * b[i];
            }

            if (normA == 0 || normB == 0)
                return 0;

            return dotProduct / (float)(Math.Sqrt(normA) * Math.Sqrt(normB));
        }

        /// <summary>
        /// Kiểm tra Ollama embeddings service có hoạt động không.
        /// </summary>
        public async Task<bool> HealthCheckAsync(CancellationToken ct = default)
        {
            try
            {
                // Test với một đoạn text ngắn
                var testEmbedding = await EmbedAsync("test", ct);
                return testEmbedding.Length > 0;
            }
            catch
            {
                return false;
            }
        }

        private void LogError(string message)
        {
            System.Diagnostics.Debug.WriteLine($"[OllamaEmbeddings] {message}");
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _httpClient?.Dispose();
                _cache.Clear();
                _disposed = true;
            }
        }

        /// <summary>
        /// Xóa cache embeddings (dùng khi data thay đổi).
        /// </summary>
        public void ClearCache()
        {
            _cache.Clear();
        }

        /// <summary>
        /// Số lượng items trong cache.
        /// </summary>
        public int CacheSize => _cache.Count;
    }
}

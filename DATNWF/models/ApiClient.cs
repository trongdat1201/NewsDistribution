using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DATNWF.Models
{
    public class ApiClient
    {
        private static ApiClient _instance;
        private readonly HttpClient _client;
        private string _token;

        public static ApiClient Instance => _instance ?? (_instance = new ApiClient());

        private ApiClient()
        {
            // Bỏ qua lỗi SSL tự ký khi chạy HTTPS ở localhost
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            _client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:7088/api/")
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void SetToken(string token)
        {
            _token = token;
            _client.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token) 
                ? null 
                : new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            HttpResponseMessage response = await _client.GetAsync(endpoint).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(json);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Phiên đăng nhập đã hết hạn hoặc không hợp lệ.");
            }
            string err = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new HttpRequestException($"Lỗi API ({response.StatusCode}): {err}");
        }

        public async Task<bool> PostAsync<T>(string endpoint, T data)
        {
            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(endpoint, content).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(endpoint, content).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string resJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<TResponse>(resJson);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Phiên đăng nhập đã hết hạn hoặc không hợp lệ.");
            }
            string err = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new HttpRequestException($"Lỗi API ({response.StatusCode}): {err}");
        }

        public async Task<bool> PutAsync<T>(string endpoint, T data)
        {
            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(endpoint, content).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            HttpResponseMessage response = await _client.DeleteAsync(endpoint).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                throw new InvalidOperationException("Không thể xóa dữ liệu vì có các ràng buộc liên quan.");
            }
            return response.IsSuccessStatusCode;
        }
    }
}

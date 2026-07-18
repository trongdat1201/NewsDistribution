using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DATNWF.Models
{
    public class ForecastClient
    {
        private static readonly HttpClient _defaultHttp;
        public static string BaseUrl { get; set; } = "http://127.0.0.1:8011";

        static ForecastClient()
        {
            _defaultHttp = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        }

        // Instance hook so unit tests can inject a mock HttpClient.
        // Production callers use the parameterless ctor and get the static default.
        private readonly HttpClient _http;

        public ForecastClient() : this(_defaultHttp) { }

        public ForecastClient(HttpClient httpClient)
        {
            _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<ForecastResult> PredictAsync(
            string maKH, string maBao, string target, string model, CancellationToken ct = default)
        {
            string url = $"{BaseUrl}/predict?kh={Uri.EscapeDataString(maKH)}&bao={Uri.EscapeDataString(maBao)}&target={Uri.EscapeDataString(target)}&model={Uri.EscapeDataString(model)}";
            using var resp = await _http.GetAsync(url, ct);
            if (!resp.IsSuccessStatusCode) return null;
            var json = await resp.Content.ReadAsStringAsync();
            var doc = JObject.Parse(json);
            if (doc["error"] != null) return null;

            return new ForecastResult
            {
                MaKhachHang = doc["MaKhachHang"]?.Value<string>(),
                MaBao = doc["MaBao"]?.Value<string>(),
                NextKyBao = doc["NextKyBao"]?.Value<int>() ?? 0,
                Model = doc["TongSoLuongBanThucTe"]?["Model"]?.Value<string>() ?? "",
                PredSLBan = doc["TongSoLuongBanThucTe"]?["Pred"]?.Value<double>() ?? 0,
                PredSLPhatHanh = doc["SoLuongPhatHanhTrongThucTe"]?["Pred"]?.Value<double>() ?? 0,
            };
        }

        /// <summary>
        /// Predict cho cap (kh, bao) tai KyBao cu the.
        /// </summary>
        public async Task<ForecastResult> PredictForKyAsync(
            string maKH, string maBao, int kyBao, string target = "both", string model = "crosslearn",
            CancellationToken ct = default)
        {
            string url = $"{BaseUrl}/predict_ky?kh={Uri.EscapeDataString(maKH)}&bao={Uri.EscapeDataString(maBao)}&ky={kyBao}&target={Uri.EscapeDataString(target)}&model={Uri.EscapeDataString(model)}";
            using var resp = await _http.GetAsync(url, ct);
            if (!resp.IsSuccessStatusCode) return null;
            var json = await resp.Content.ReadAsStringAsync();
            var doc = JObject.Parse(json);
            if (doc["error"] != null) return null;

            return new ForecastResult
            {
                MaKhachHang = doc["MaKhachHang"]?.Value<string>(),
                MaBao = doc["MaBao"]?.Value<string>(),
                KyBao = doc["KyBao"]?.Value<int>() ?? kyBao,
                Model = doc["TongSoLuongBanThucTe"]?["Model"]?.Value<string>() ?? "",
                PredSLBan = doc["TongSoLuongBanThucTe"]?["Pred"]?.Value<double>() ?? 0,
                PredSLPhatHanh = doc["SoLuongPhatHanhTrongThucTe"]?["Pred"]?.Value<double>() ?? 0,
                IsActual = doc["TongSoLuongBanThucTe"]?["is_actual"]?.Value<bool>() ?? false,
            };
        }

        /// <summary>
        /// Tao URL batch voi ky va ngay phat hanh can thang hang theo vi tri.
        /// </summary>
        public static string BuildBatchPredictionUrl(
            string maKH, string maBao, int[] kyList, DateTime[] dateList,
            string target = "both", string model = "crosslearn")
        {
            if (string.IsNullOrWhiteSpace(maKH))
                throw new ArgumentException("maKH khong duoc de trong", nameof(maKH));
            if (string.IsNullOrWhiteSpace(maBao))
                throw new ArgumentException("maBao khong duoc de trong", nameof(maBao));
            if (kyList == null || kyList.Length == 0)
                throw new ArgumentException("kyList phai co it nhat mot phan tu", nameof(kyList));
            if (dateList == null || dateList.Length != kyList.Length)
                throw new ArgumentException("dateList phai co cung so phan tu voi kyList", nameof(dateList));

            string kyListStr = string.Join(",", kyList);
            string[] isoDates = new string[dateList.Length];
            for (int i = 0; i < dateList.Length; i++)
                isoDates[i] = dateList[i].ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string dateListStr = string.Join(",", isoDates);

            return $"{BaseUrl}/predict_ky_batch?kh={Uri.EscapeDataString(maKH)}" +
                   $"&bao={Uri.EscapeDataString(maBao)}" +
                   $"&ky_list={Uri.EscapeDataString(kyListStr)}" +
                   $"&date_list={Uri.EscapeDataString(dateListStr)}" +
                   $"&target={Uri.EscapeDataString(target)}" +
                   $"&model={Uri.EscapeDataString(model)}";
        }

        /// <summary>
        /// Predict nhieu KyBao cung luc cho 1 cap (kh, bao).
        /// Tra ve danh sach predictions.
        /// </summary>
        public async Task<ForecastBatchResult> PredictBatchForKyAsync(
            string maKH, string maBao, int[] kyList, DateTime[] dateList,
            string target = "both", string model = "crosslearn",
            CancellationToken ct = default)
        {
            string url = BuildBatchPredictionUrl(maKH, maBao, kyList, dateList, target, model);
            using var resp = await _http.GetAsync(url, ct);
            if (!resp.IsSuccessStatusCode) return null;
            var json = await resp.Content.ReadAsStringAsync();
            var doc = JObject.Parse(json);
            if (doc["error"] != null) return null;

            var result = new ForecastBatchResult
            {
                MaKhachHang = doc["MaKhachHang"]?.Value<string>(),
                MaBao = doc["MaBao"]?.Value<string>(),
                Predictions = new System.Collections.Generic.List<ForecastKyPrediction>(),
            };
            var byKy = new System.Collections.Generic.Dictionary<int, ForecastKyPrediction>();

            var soldPredictions = doc["predictions"]?["TongSoLuongBanThucTe"];
            if (soldPredictions != null)
            {
                foreach (var prediction in soldPredictions)
                {
                    int kyBao = prediction["KyBao"]?.Value<int>() ?? 0;
                    var item = new ForecastKyPrediction
                    {
                        KyBao = kyBao,
                        PredSLBan = prediction["Pred"]?.Value<double>() ?? 0,
                        NgayNhan = ParseIsoDate(prediction["NgayNhan"]?.Value<string>()),
                        IsActual = prediction["is_actual"]?.Value<bool>() ?? false,
                    };
                    result.Predictions.Add(item);
                    byKy[kyBao] = item;
                }
            }

            var issuedPredictions = doc["predictions"]?["SoLuongPhatHanhTrongThucTe"];
            if (issuedPredictions != null)
            {
                foreach (var prediction in issuedPredictions)
                {
                    int kyBao = prediction["KyBao"]?.Value<int>() ?? 0;
                    if (!byKy.TryGetValue(kyBao, out var item))
                    {
                        item = new ForecastKyPrediction
                        {
                            KyBao = kyBao,
                            NgayNhan = ParseIsoDate(prediction["NgayNhan"]?.Value<string>()),
                            IsActual = prediction["is_actual"]?.Value<bool>() ?? false,
                        };
                        result.Predictions.Add(item);
                        byKy[kyBao] = item;
                    }
                    item.PredSLPhatHanh = prediction["Pred"]?.Value<double>() ?? 0;
                }
            }

            return result;
        }

        private static DateTime ParseIsoDate(string value)
        {
            return DateTime.TryParseExact(
                value,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime parsed)
                ? parsed
                : DateTime.MinValue;
        }
    }

    public class ForecastResult
    {
        public string MaKhachHang { get; set; }
        public string MaBao { get; set; }
        public int NextKyBao { get; set; }
        public int KyBao { get; set; }  // KyBao khi dung predict_ky
        public string Model { get; set; }
        public double PredSLBan { get; set; }
        public double PredSLPhatHanh { get; set; }
        public bool IsActual { get; set; }  // True neu la gia tri thuc tu history
    }

    public class ForecastBatchResult
    {
        public string MaKhachHang { get; set; }
        public string MaBao { get; set; }
        public System.Collections.Generic.List<ForecastKyPrediction> Predictions { get; set; }
    }
}

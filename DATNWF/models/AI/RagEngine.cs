using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DATNWF.Models;

namespace DATNWF.Models.AI
{
    /// <summary>
    /// RAG Engine: searches the database for relevant context and injects
    /// it into the user prompt before sending to the AI provider.
    /// Hỗ trợ cả SQL search và vector search (Ollama embeddings).
    /// </summary>
    public class RagEngine
    {
        private readonly DbHelper _db;
        private readonly OllamaEmbeddings _embeddings;

        // Simple in-memory cache for embeddings (TTL: 5 minutes)
        private readonly Dictionary<string, (float[] embedding, DateTime cachedAt)> _embeddingCache = new Dictionary<string, (float[], DateTime)>();
        private readonly object _cacheLock = new object();
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

        public RagEngine()
        {
            _db = DbHelper.Instance;
            _embeddings = new OllamaEmbeddings();
        }

        /// <summary>
        /// Lấy embedding với in-memory cache (5 phút TTL).
        /// Giảm API calls từ O(n) xuống O(1) cho các query trùng lặp.
        /// </summary>
        private async Task<float[]> GetCachedEmbeddingAsync(string text, CancellationToken ct)
        {
            string key = text.ToLowerInvariant().Trim();

            lock (_cacheLock)
            {
                if (_embeddingCache.TryGetValue(key, out var cached))
                {
                    if (DateTime.UtcNow - cached.cachedAt < _cacheDuration)
                        return cached.embedding;
                    _embeddingCache.Remove(key);
                }
            }

            var embedding = await _embeddings.EmbedAsync(text, ct);

            if (embedding.Length > 0)
            {
                lock (_cacheLock)
                {
                    _embeddingCache[key] = (embedding, DateTime.UtcNow);
                }
            }

            return embedding;
        }

        /// <summary>
        /// Main entry point: given a user query, search DB and return
        /// relevant context as a string, trimmed to MaxRagContextChars.
        ///
        /// Domain queries are dispatched concurrently to reduce total latency
        /// when the user asks about multiple areas at once.
        /// </summary>
        public async Task<string> BuildContextAsync(string userQuery, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(userQuery) || !AiConfig.RagEnabled)
                return string.Empty;

            var queries = ParseQuery(userQuery);
            if (queries.Count == 0)
                return string.Empty;

            // Fire each domain lookup in parallel — independent SQL queries.
            var tasks = queries.Select(q => DispatchDomainAsync(q.domain, q.keyword, ct)).ToArray();

            // All exceptions are caught per-call inside DispatchDomainAsync so Task.WhenAll won't fault.
            string[] sections = await Task.WhenAll(tasks);

            var nonEmpty = sections.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            if (nonEmpty.Count == 0)
                return string.Empty;

            var context = new StringBuilder();
            context.AppendLine("=== NGỮ CẢNH TỪ CƠ SỞ DỮ LIỆU ===");
            context.AppendLine("(Dữ liệu dưới đây được trích xuất tự động từ hệ thống — bạn có thể dùng để trả lời chính xác)");
            context.AppendLine();

            foreach (var s in nonEmpty)
            {
                context.AppendLine(s);
                context.AppendLine();
            }

            string result = context.ToString().TrimEnd();

            if (result.Length > AiConfig.MaxRagContextChars)
                result = result.Substring(0, AiConfig.MaxRagContextChars) + "\n\n[...RAG context bị cắt ngắn do quá dài...]";

            return result;
        }

        /// <summary>
        /// Routes a (domain, keyword) pair to its concrete search method and
        /// guarantees no exception escapes — surface them as error text instead.
        /// </summary>
        private async Task<string> DispatchDomainAsync(string domain, string keyword, CancellationToken ct)
        {
            try
            {
                return domain switch
                {
                    "customer" => await SearchCustomersRagAsync(keyword, ct),
                    "publication" => await SearchPublicationsRagAsync(keyword, ct),
                    "invoice" => await SearchInvoicesRagAsync(keyword, ct),
                    "inventory" => await SearchInventoryRagAsync(keyword, ct),
                    "dashboard" => await GetDashboardRagAsync(ct),
                    "semantic" => await SemanticSearchAsync(keyword, ct),
                    _ => string.Empty
                };
            }
            catch (OperationCanceledException)
            {
                throw; // Honour cancellation tokens for the consumer.
            }
            catch (Exception ex)
            {
                return $"[RAG error] Domain '{domain}' failed: {ex.Message}";
            }
        }

        /// <summary>
        /// Semantic search sử dụng Ollama embeddings với caching.
        /// Giảm từ 150 API calls xuống còn 1 (query) + cache hits.
        /// </summary>
        private async Task<string> SemanticSearchAsync(string query, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(query) || !AiConfig.UseOllamaEmbeddings)
                return string.Empty;

            try
            {
                // Tạo embedding cho query (đã có cache)
                var queryEmbedding = await GetCachedEmbeddingAsync(query, ct);
                if (queryEmbedding.Length == 0)
                    return string.Empty;

                // Tìm kiếm SQL để lấy data candidates (giảm từ TOP 15 xuống TOP 15)
                var allData = await GetAllSearchableDataAsync(ct);

                if (allData.Count == 0)
                    return string.Empty;

                // Tính similarity và sort (dùng cached embeddings)
                var scored = new List<(string text, float score, string source)>();

                foreach (var item in allData)
                {
                    var itemEmbedding = await GetCachedEmbeddingAsync(item.text, ct);
                    if (itemEmbedding.Length > 0)
                    {
                        var similarity = OllamaEmbeddings.CosineSimilarity(queryEmbedding, itemEmbedding);
                        if (similarity > 0.5f) // Threshold
                        {
                            scored.Add((item.text, similarity, item.source));
                        }
                    }
                }

                // Sort by similarity descending
                var topResults = scored
                    .OrderByDescending(x => x.score)
                    .Take(5)
                    .ToList();

                if (topResults.Count == 0)
                    return string.Empty;

                var lines = new List<string> { "[Tìm kiếm ngữ nghĩa] Kết quả liên quan:" };
                foreach (var result in topResults)
                {
                    lines.Add($"  [{result.score:P0}] {result.text}");
                }

                return string.Join("\n", lines);
            }
            catch (Exception ex)
            {
                return $"[Semantic search error] {ex.Message}";
            }
        }

        private class SearchableItem
        {
            public string text;
            public string source;
        }

        private async Task<List<SearchableItem>> GetAllSearchableDataAsync(CancellationToken ct)
        {
            // Chạy song song để giảm thời gian
            var taskCustomers = GetCustomersAsync(ct);
            var taskPublications = GetPublicationsAsync(ct);
            var taskInvoices = GetInvoicesAsync(ct);

            await Task.WhenAll(taskCustomers, taskPublications, taskInvoices);

            var results = new List<SearchableItem>();
            results.AddRange(taskCustomers.Result);
            results.AddRange(taskPublications.Result);
            results.AddRange(taskInvoices.Result);

            return results;
        }

        private async Task<List<SearchableItem>> GetCustomersAsync(CancellationToken ct)
        {
            var list = new List<SearchableItem>();
            try
            {
                var dt = await _db.FillDataTableAsync(
                    "SELECT TOP 15 Makh, Ten, Dienthoai, Diachi FROM TabKhachhang ORDER BY Ten",
                    ct);

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new SearchableItem
                    {
                        text = $"Khách hàng {r["Ten"]}, Mã: {r["Makh"]}, ĐT: {r["Dienthoai"]}, Địa chỉ: {r["Diachi"]}",
                        source = "customer"
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RagEngine] GetCustomersAsync error: {ex.Message}");
            }
            return list;
        }

        private async Task<List<SearchableItem>> GetPublicationsAsync(CancellationToken ct)
        {
            var list = new List<SearchableItem>();
            try
            {
                var dt = await _db.FillDataTableAsync(
                    "SELECT TOP 15 MaBao, Ten, Dvt, DonGia FROM TabBao ORDER BY Ten",
                    ct);

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new SearchableItem
                    {
                        text = $"Đầu báo {r["Ten"]}, Mã: {r["MaBao"]}, Đơn vị: {r["Dvt"]}, Giá: {r["DonGia"]}",
                        source = "publication"
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RagEngine] GetPublicationsAsync error: {ex.Message}");
            }
            return list;
        }

        private async Task<List<SearchableItem>> GetInvoicesAsync(CancellationToken ct)
        {
            var list = new List<SearchableItem>();
            try
            {
                var dt = await _db.FillDataTableAsync(
                    @"SELECT TOP 15 hd.Sohd, hd.NgayLapPhieu, kh.Ten, hd.ThanhToan 
                      FROM TabHoadon hd INNER JOIN TabKhachhang kh ON hd.Makh = kh.Makh 
                      ORDER BY hd.NgayLapPhieu DESC",
                    ct);

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new SearchableItem
                    {
                        text = $"Hóa đơn {r["Sohd"]}, Khách: {r["Ten"]}, Ngày: {r["NgayLapPhieu"]}, TT: {(r["ThanhToan"] == DBNull.Value || !(bool)r["ThanhToan"] ? "Chưa thanh toán" : "Đã thanh toán")}",
                        source = "invoice"
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RagEngine] GetInvoicesAsync error: {ex.Message}");
            }
            return list;
        }

        /// <summary>
        /// Detect which domain(s) the query is about and extract keywords.
        /// Sử dụng expanded synonyms và fuzzy matching để handle typos và đồng nghĩa.
        /// </summary>
        private List<(string domain, string keyword)> ParseQuery(string query)
        {
            var results = new List<(string, string)>();
            string q = query.ToLowerInvariant().Trim();

            // === SEMANTIC SEARCH ===
            // User muốn tìm kiếm ngữ nghĩa - dùng Ollama embeddings
            string[] semanticKeywords = { "tìm", "tim", "search", "tìm kiếm", "liên quan", "quan tâm", "gợi ý", "suggest", "tra cứu", "kiếm", "tìm thấy" };
            if (ContainsAny(q, semanticKeywords) && AiConfig.UseOllamaEmbeddings)
            {
                string searchQuery = ExtractSearchQuery(q);
                results.Add(("semantic", searchQuery));
            }

            // === DASHBOARD / TỔNG QUAN ===
            string[] dashboardKeywords = { "tổng quan", "dashboard", "thống kê", "tổng hợp", "số liệu", "doanh thu", "biểu đồ", "tổng", "số lượng", "bao nhiêu", "còn bao nhiêu" };
            if (ContainsAny(q, dashboardKeywords))
            {
                results.Add(("dashboard", string.Empty));
            }

            // === CUSTOMERS / KHÁCH HÀNG ===
            // Bao gồm: tên khách, mã KH, điện thoại, địa chỉ, loại P_PH, P_KT
            string[] customerKeywords = { 
                "khách hàng", "khach hang", "khách", "khach", "mã kh", "ma kh", "tên kh", "ten kh",
                "khách hàng phát hành", "kh phát hành", "khách p_ph", "kh p_ph",
                "khách kỹ thuật", "kh kỹ thuật", "khách kt", "kh kt",
                "đại lý", "đại lý báo", "đại lý phát hành",
                "người mua", "người nhận", "cửa hàng"
            };
            if (ContainsAny(q, customerKeywords) || FuzzyContains(q, "khách"))
            {
                string kw = ExtractKeyword(q, customerKeywords);
                results.Add(("customer", kw));
            }

            // === PUBLICATIONS / BÁO TẠP CHÍ ===
            string[] publicationKeywords = { 
                "báo", "tap chí", "tạp chí", "tạp chi", "báo nào", "mã báo", "tên báo", 
                "đơn giá", "giá báo", "báo giá", "loại báo", "danh mục báo",
                "thanh niên", "tuổi trẻ", "thể thao", "văn nghệ", "vnexpress", "dân trí"
            };
            if (ContainsAny(q, publicationKeywords) || FuzzyContains(q, "báo"))
            {
                string kw = ExtractKeyword(q, publicationKeywords);
                results.Add(("publication", kw));
            }

            // === INVOICES / HÓA ĐƠN ===
            // Bao gồm: hóa đơn, phiếu thu, phiếu chi, số HD, invoice
            string[] invoiceKeywords = { 
                "hóa đơn", "hoá đơn", "hoá dơn", "hoadon", "hóadon", "hđ", "hd",
                "số hđ", "so hd", "số hd", "so hđ",
                "phiếu thu", "phieu thu", "phiếu chi", "phieu chi",
                "invoice", "bill", "receipt"
            };
            if (ContainsAny(q, invoiceKeywords) || FuzzyContains(q, "hóa đơn"))
            {
                string kw = ExtractKeyword(q, invoiceKeywords);
                results.Add(("invoice", kw));
            }

            // === INVENTORY / TỒN KHO ===
            string[] inventoryKeywords = { 
                "tồn kho", "ton kho", "tồn", "hàng tồn", "hang ton",
                "số lượng", "so luong", "sl", "số lượng tồn",
                "bán thực", "ban thuc", "bán lẻ", "ban le",
                "phát hành", "phat hanh", "phát hành", "xuất bán",
                "nhập kho", "xuất kho", "nhập hàng"
            };
            if (ContainsAny(q, inventoryKeywords) || FuzzyContains(q, "tồn"))
            {
                string kw = ExtractKeyword(q, inventoryKeywords);
                results.Add(("inventory", kw));
            }

            // Deduplicate — keep one per domain
            var seen = new HashSet<string>();
            var deduped = new List<(string, string)>();
            foreach (var r in results)
            {
                if (seen.Add(r.Item1))
                    deduped.Add(r);
            }
            return deduped;
        }

        private static string ExtractSearchQuery(string q)
        {
            foreach (var term in new[] { "tìm", "tim", "search", "tìm kiếm", "liên quan", "gợi ý" })
            {
                int idx = q.IndexOf(term, StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                {
                    string after = q.Substring(idx + term.Length).Trim();
                    after = after.TrimStart(':', ' ', '-', ' ');
                    if (!string.IsNullOrWhiteSpace(after))
                        return after;
                }
            }
            return q;
        }

        private static bool Contains(string q, params string[] terms)
        {
            foreach (var t in terms)
            {
                if (q.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            return false;
        }

        private static bool ContainsAny(string q, string[] terms)
        {
            foreach (var t in terms)
            {
                if (q.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Fuzzy matching sử dụng Levenshtein distance.
        /// Cho phép match với độ sai khác tối đa 20%.
        /// </summary>
        private static bool FuzzyContains(string q, string term)
        {
            // Quick check: nếu có chứa từ đúng, return true
            if (q.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                return true;

            // Fuzzy check: so sánh với Levenshtein distance
            int threshold = Math.Max(1, (int)(term.Length * 0.2)); // 20% tolerance

            foreach (var word in q.Split(new[] { ' ', ',', '.', '?', '!', ';', ':' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (word.Length < 2) continue;
                int distance = LevenshteinDistance(word, term);
                if (distance <= threshold)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Tính Levenshtein distance giữa 2 strings.
        /// </summary>
        private static int LevenshteinDistance(string s1, string s2)
        {
            int[,] d = new int[s1.Length + 1, s2.Length + 1];

            for (int i = 0; i <= s1.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= s2.Length; j++) d[0, j] = j;

            for (int j = 1; j <= s2.Length; j++)
            {
                for (int i = 1; i <= s1.Length; i++)
                {
                    int cost = s1[i - 1] == s2[j - 1] ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[s1.Length, s2.Length];
        }

        private static string ExtractKeyword(string q, params string[] prefixes)
        {
            foreach (var p in prefixes)
            {
                int idx = q.IndexOf(p, StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                {
                    string after = q.Substring(idx + p.Length).Trim();
                    after = after.TrimStart(':', ' ', '-');
                    int end = after.Length;
                    for (int i = 0; i < after.Length; i++)
                    {
                        if (".,;!?".IndexOf(after[i]) >= 0) { end = i; break; }
                    }
                    string kw = after.Substring(0, end).Trim();
                    if (!string.IsNullOrWhiteSpace(kw) && kw.Length > 1)
                        return kw;
                }
            }
            return string.Empty;
        }

        #region Individual search methods

        private async Task<string> SearchCustomersRagAsync(string keyword, CancellationToken ct)
        {
            string sql = @"SELECT TOP 10 Makh, Ten, Dienthoai, Diachi, Chietkhau
                           FROM TabKhachhang WHERE 1=1";

            var ps = new List<System.Data.SqlClient.SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += " AND (Ten LIKE @kw OR Dienthoai LIKE @kw OR Makh LIKE @kw)";
                ps.Add(new System.Data.SqlClient.SqlParameter("@kw", System.Data.SqlDbType.NVarChar) { Value = $"%{keyword}%" });
            }

            sql += " ORDER BY Ten";

            var dt = await _db.FillDataTableAsync(sql, ct, ps.ToArray());

            if (dt.Rows.Count == 0)
                return $"[Khách hàng] Không tìm thấy khách hàng nào.";

            var lines = new List<string> { $"[Khách hàng] Tìm thấy {dt.Rows.Count} khách hàng:" };
            foreach (DataRow r in dt.Rows)
            {
                string ck = r["Chietkhau"] != DBNull.Value ? Convert.ToInt16(r["Chietkhau"]) + "%" : "0%";
                lines.Add($"  • {r["Makh"]} | {r["Ten"]} | ĐT: {r["Dienthoai"]} | CK: {ck}");
            }
            return string.Join("\n", lines);
        }

        private async Task<string> SearchPublicationsRagAsync(string keyword, CancellationToken ct)
        {
            string sql = @"SELECT TOP 10 MaBao, Ten, Dvt, DonGia, SoLanPhtrongTuan
                           FROM TabBao WHERE 1=1";

            var ps = new List<System.Data.SqlClient.SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += " AND Ten LIKE @kw";
                ps.Add(new System.Data.SqlClient.SqlParameter("@kw", System.Data.SqlDbType.NVarChar) { Value = $"%{keyword}%" });
            }

            sql += " ORDER BY Ten";

            var dt = await _db.FillDataTableAsync(sql, ct, ps.ToArray());

            if (dt.Rows.Count == 0)
                return $"[Báo/Tạp chí] Không tìm thấy báo nào.";

            var lines = new List<string> { $"[Báo/Tạp chí] Tìm thấy {dt.Rows.Count} đầu báo:" };
            foreach (DataRow r in dt.Rows)
            {
                string gia = r["DonGia"] != DBNull.Value ? Convert.ToDouble(r["DonGia"]).ToString("N0") + "đ" : "N/A";
                string lan = r["SoLanPhtrongTuan"] != DBNull.Value ? Convert.ToInt32(r["SoLanPhtrongTuan"]) + " lần/tuần" : "N/A";
                lines.Add($"  • {r["MaBao"]} | {r["Ten"]} | {gia} | {lan}");
            }
            return string.Join("\n", lines);
        }

        private async Task<string> SearchInvoicesRagAsync(string keyword, CancellationToken ct)
        {
            string sql = @"SELECT TOP 10 hd.Sohd, hd.NgayLapPhieu, kh.Ten, hd.ThanhToan
                           FROM TabHoadon hd
                           INNER JOIN TabKhachhang kh ON hd.Makh = kh.Makh
                           WHERE 1=1";

            var ps = new List<System.Data.SqlClient.SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += " AND (hd.Sohd LIKE @kw OR kh.Ten LIKE @kw OR kh.Makh LIKE @kw)";
                ps.Add(new System.Data.SqlClient.SqlParameter("@kw", System.Data.SqlDbType.NVarChar) { Value = $"%{keyword}%" });
            }

            sql += " ORDER BY hd.NgayLapPhieu DESC";

            var dt = await _db.FillDataTableAsync(sql, ct, ps.ToArray());

            if (dt.Rows.Count == 0)
                return $"[Hóa đơn] Không tìm thấy hóa đơn nào.";

            var lines = new List<string> { $"[Hóa đơn] Tìm thấy {dt.Rows.Count} hóa đơn gần nhất:" };
            foreach (DataRow r in dt.Rows)
            {
                string ngay = r["NgayLapPhieu"] != DBNull.Value
                    ? ((DateTime)r["NgayLapPhieu"]).ToString("dd/MM/yyyy")
                    : "N/A";
                string tt = r["ThanhToan"] != DBNull.Value && (bool)r["ThanhToan"]
                    ? "✓ Đã thanh toán"
                    : "✗ Chưa thanh toán";
                lines.Add($"  • {r["Sohd"]} | {r["Ten"]} | {ngay} | {tt}");
            }
            return string.Join("\n", lines);
        }

        private async Task<string> SearchInventoryRagAsync(string keyword, CancellationToken ct)
        {
            string sql = @"SELECT TOP 10 t.MaBao, b.Ten, t.Ngay, t.SoBao, t.Banthuc, t.Ton
                           FROM TabTon t
                           INNER JOIN TabBao b ON t.MaBao = b.MaBao
                           WHERE 1=1";

            var ps = new List<System.Data.SqlClient.SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += " AND (b.Ten LIKE @kw OR b.MaBao LIKE @kw)";
                ps.Add(new System.Data.SqlClient.SqlParameter("@kw", System.Data.SqlDbType.NVarChar) { Value = $"%{keyword}%" });
            }

            sql += " ORDER BY t.Ngay DESC";

            var dt = await _db.FillDataTableAsync(sql, ct, ps.ToArray());

            if (dt.Rows.Count == 0)
                return $"[Tồn kho] Không có dữ liệu tồn kho.";

            var lines = new List<string> { $"[Tồn kho] Tìm thấy {dt.Rows.Count} bản ghi:" };
            foreach (DataRow r in dt.Rows)
            {
                string ngay = r["Ngay"] != DBNull.Value
                    ? ((DateTime)r["Ngay"]).ToString("dd/MM/yyyy")
                    : "N/A";
                lines.Add($"  • {r["Ten"]} | {ngay} | Phát hành: {r["SoBao"]} | Bán thực: {r["Banthuc"]} | Tồn: {r["Ton"]}");
            }
            return string.Join("\n", lines);
        }

        private async Task<string> GetDashboardRagAsync(CancellationToken ct)
        {
            var lines = new List<string> { "[Dashboard]" };

            try
            {
                decimal dt = await _db.ExecuteScalarAsync<decimal>(
                    @"SELECT ISNULL(SUM(ct.SoLuongThuc * b.DonGia), 0)
                      FROM TabHoadon hd
                      INNER JOIN TabChitiethoadon ct ON hd.Sohd = ct.Sohd
                      INNER JOIN TabBao b ON ct.MaBao = b.MaBao
                      WHERE hd.ThanhToan = 1", ct);
                lines.Add($"  • Tổng doanh thu: {dt:N0}đ");
            }
            catch (Exception ex)
            {
                lines.Add($"  • Tổng doanh thu: lỗi — {ex.Message}");
            }

            try
            {
                int cb = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM TabBao", ct);
                lines.Add($"  • Tổng đầu báo: {cb}");
            }
            catch (Exception ex)
            {
                lines.Add($"  • Tổng đầu báo: lỗi — {ex.Message}");
            }

            try
            {
                int kh = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM TabKhachhang", ct);
                lines.Add($"  • Tổng khách hàng: {kh}");
            }
            catch (Exception ex)
            {
                lines.Add($"  • Tổng khách hàng: lỗi — {ex.Message}");
            }

            return string.Join("\n", lines);
        }

        #endregion

        /// <summary>
        /// Returns a business schema description for system prompt injection.
        /// </summary>
        public string GetBusinessSchema()
        {
            return @"DATNWF là hệ thống quản lý phân phối báo/tạp chí.
Các bảng chính:
- TabKhachhang(Makh, Ten, Diachi, Dienthoai, Chietkhau, P_PH, P_KT, Uutien) — thông tin khách hàng
- TabBao(MaBao, Ten, Dvt, DonGia, NgayBatDau, Thu1..Thu7, SoLanPhtrongTuan, Sogoc) — danh mục báo
- TabHoadon(Sohd, Makh, NgayLapPhieu, TuNgay, DenNgay, ThanhToan) — hóa đơn
- TabTon(Ngay, MaBao, SoBao, SlPhatHanh, Banthuc, BanLe, DieuPhoi, Ton) — tồn kho
- TabChitiethoadon(Sohd, MaBao, SoLuongThuc, DonGia) — chi tiết hóa đơn";
        }
    }
}

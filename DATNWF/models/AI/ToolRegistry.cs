using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DATNWF.Models;

namespace DATNWF.Models.AI
{
    public class ToolRegistry
    {
        public List<ToolDefinition> GetAllDefinitions()
        {
            return new List<ToolDefinition>
            {
                new ToolDefinition
                {
                    Name = "SearchCustomers",
                    Description = "Tìm và đếm khách hàng theo tên, số điện thoại, hoặc loại (P_PH / P_KT). Luôn trả về số lượng. Dùng khi user hỏi về khách hàng, số lượng khách hàng, phân loại.",
                    Parameters = new Dictionary<string, ToolParameter>
                    {
                        ["keyword"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Từ khóa tìm kiếm (tên hoặc số điện thoại). Để trống nếu chỉ lọc theo loại."
                        },
                        ["loaiKh"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Loại khách hàng: 'P_PH' (phát hành), 'P_KT' (kỹ thuật), 'P_PH_P_KT' (cả hai). Để trống để lấy tất cả."
                        }
                    }
                },
                new ToolDefinition
                {
                    Name = "SearchPublications",
                    Description = "Tìm báo/tạp chí theo tên. Trả về thông tin đơn giá và ngày bắt đầu phát hành.",
                    Parameters = new Dictionary<string, ToolParameter>
                    {
                        ["keyword"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Tên báo/tạp chí cần tìm. Để trống sẽ trả về danh sách tất cả các báo."
                        }
                    }
                },
                new ToolDefinition
                {
                    Name = "SearchInvoices",
                    Description = "Tìm hóa đơn theo số hóa đơn, tên khách hàng hoặc ngày lập. Trả về thông tin chi tiết hóa đơn.",
                    Parameters = new Dictionary<string, ToolParameter>
                    {
                        ["soHd"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Số hóa đơn (ví dụ: HD-2026-0001). Có thể để trống."
                        },
                        ["tenKhachHang"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Tên hoặc mã khách hàng liên quan đến hóa đơn. Có thể để trống."
                        },
                        ["ngay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Ngày lập hóa đơn theo định dạng yyyy-MM-dd (ví dụ: 2026-01-15). Có thể để trống."
                        }
                    }
                },
                new ToolDefinition
                {
                    Name = "GetInventory",
                    Description = "Xem tình trạng tồn kho báo/tạp chí. Trả về số lượng phát hành, bán thực tế, bán lẻ, điều phối và tồn kho hiện tại.",
                    Parameters = new Dictionary<string, ToolParameter>
                    {
                        ["maBao"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Mã báo/tạp chí (ví dụ: BAO001). Có thể để trống để xem tất cả."
                        },
                        ["tenBao"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Tên báo/tạp chí cần xem tồn kho. Có thể để trống."
                        }
                    }
                },
                new ToolDefinition
                {
                    Name = "GetDashboardSummary",
                    Description = "Lấy tóm tắt dashboard: tổng doanh thu, số lượng báo, số khách hàng, biểu đồ thống kê. Không cần tham số.",
                    Parameters = new Dictionary<string, ToolParameter>()
                },
                new ToolDefinition
                {
                    Name = "GetTopCustomersByRevenue",
                    Description = "Xếp hạng khách hàng theo tổng doanh thu (đã thanh toán) trong khoảng thời gian. Dùng khi user hỏi 'khách nào mua nhiều nhất', 'top khách hàng doanh thu cao', 'VIP'.",
                    Parameters = new Dictionary<string, ToolParameter>
                    {
                        ["topN"] = new ToolParameter
                        {
                            Type = "number",
                            Description = "Số lượng khách hàng top đầu cần lấy. Mặc định 10, tối đa 50."
                        },
                        ["tuNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Lọc từ ngày (yyyy-MM-dd). Để trống = tất cả."
                        },
                        ["denNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Lọc đến ngày (yyyy-MM-dd). Để trống = đến hiện tại."
                        }
                    }
                },
                new ToolDefinition
                {
                    Name = "GetTopPublicationsByRevenue",
                    Description = "Xếp hạng báo/tạp chí theo tổng doanh thu hoặc tổng số lượng bán. Dùng khi user hỏi 'báo nào bán chạy', 'đầu báo nào hot', 'sản phẩm chủ lực'.",
                    Parameters = new Dictionary<string, ToolParameter>
                    {
                        ["topN"] = new ToolParameter
                        {
                            Type = "number",
                            Description = "Số lượng báo top đầu. Mặc định 10, tối đa 50."
                        },
                        ["tuNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Lọc từ ngày (yyyy-MM-dd). Để trống = tất cả."
                        },
                        ["denNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Lọc đến ngày (yyyy-MM-dd). Để trống = đến hiện tại."
                        },
                        ["theoSoLuong"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "'true' = xếp theo tổng số lượng bán (soLuongThuc), 'false' = xếp theo doanh thu (thanhTien). Mặc định 'false'."
                        }
                    }
                },
                new ToolDefinition
                {
                    Name = "GetRevenueByPeriod",
                    Description = "Thống kê doanh thu theo khoảng thời gian (ngày/tháng/quý/năm). Dùng khi user hỏi 'doanh thu tháng X', 'so sánh doanh thu các tháng', 'báo cáo tài chính'.",
                    Parameters = new Dictionary<string, ToolParameter>
                    {
                        ["tuNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Từ ngày (yyyy-MM-dd). Bắt buộc."
                        },
                        ["denNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Đến ngày (yyyy-MM-dd). Bắt buộc."
                        },
                        ["groupBy"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Gom nhóm theo: 'day' (theo ngày), 'month' (theo tháng), 'year' (theo năm). Mặc định 'day'."
                        }
                    }
                },
                new ToolDefinition
                {
                    Name = "GetOverdueInvoices",
                    Description = "Liệt kê hóa đơn chưa thanh toán hoặc quá hạn. Dùng khi user hỏi 'công nợ', 'ai chưa trả tiền', 'hóa đơn treo'.",
                    Parameters = new Dictionary<string, ToolParameter>
                    {
                        ["topN"] = new ToolParameter
                        {
                            Type = "number",
                            Description = "Số hóa đơn cần liệt kê. Mặc định 20, tối đa 100."
                        },
                        ["chinhXacQuaHan"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "'true' = chỉ lấy hóa đơn quá hạn (DenNgay < hôm nay), 'false' = tất cả chưa thanh toán. Mặc định 'false'."
                        }
                    }
                },
                new ToolDefinition
                {
                    Name = "GetLowInventory",
                    Description = "Cảnh báo tồn kho thấp/sắp hết. Dùng khi user hỏi 'báo nào sắp hết', 'tồn kho cần nhập', 'low stock'.",
                    Parameters = new Dictionary<string, ToolParameter>
                    {
                        ["nguongTon"] = new ToolParameter
                        {
                            Type = "number",
                            Description = "Ngưỡng tồn kho tối đa. Lấy các báo có Ton <= ngưỡng. Mặc định 10."
                        },
                        ["ngay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Xem tồn kho ngày nào (yyyy-MM-dd). Để trống = ngày gần nhất có dữ liệu."
                        }
                    }
                },
                new ToolDefinition
                {
                    Name = "GetCustomerActivity",
                    Description = "Xem hoạt động mua hàng của 1 khách cụ thể: tổng doanh thu, số hóa đơn, top báo đã mua. Dùng khi user hỏi 'khách X mua gì', 'lịch sử KH X', 'phân tích KH X'.",
                    Parameters = new Dictionary<string, ToolParameter>
                    {
                        ["maKhachHang"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Mã khách hàng (MaKH). Bắt buộc."
                        },
                        ["tenKhachHang"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Tên khách hàng (dùng khi không biết MaKH, sẽ tìm tên tương tự)."
                        },
                        ["tuNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Từ ngày (yyyy-MM-dd). Để trống = tất cả."
                        },
                        ["denNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Đến ngày (yyyy-MM-dd). Để trống = đến hiện tại."
                        }
                    }
                },
                new ToolDefinition
                {
                    Name = "GetCustomerInventoryWaste",
                    Description = "Khách hàng nào có số lượng báo dư (tồn) nhiều nhất - tức là SL phát cho KH nhưng KH không bán hết phải trả lại. Dùng khi user hỏi 'khách nào tồn nhiều', 'KH nào hay trả lại báo', 'số lượng dư', 'SL dư', 'phát mà không bán hết'.",
                    Parameters = new Dictionary<string, ToolParameter>
                    {
                        ["topN"] = new ToolParameter
                        {
                            Type = "integer",
                            Description = "Số khách hàng muốn xem (mặc định 10, tối đa 50)."
                        },
                        ["tuNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Từ ngày (yyyy-MM-dd). Để trống = tất cả."
                        },
                        ["denNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Đến ngày (yyyy-MM-dd). Để trống = đến hiện tại."
                        }
                    }
                },
                new ToolDefinition
                {
                    Name = "GetPublicationsByWasteRate",
                    Description = "Đầu báo nào có tỷ lệ tồn (SL dư / tổng SL phát) cao nhất - tức là báo bị trả lại nhiều. Dùng khi user hỏi 'báo nào tồn nhiều nhất', 'tỷ lệ waste', 'báo nào hay bị trả lại'.",
                    Parameters = new Dictionary<string, ToolParameter>
                    {
                        ["topN"] = new ToolParameter
                        {
                            Type = "integer",
                            Description = "Số đầu báo muốn xem (mặc định 10, tối đa 50)."
                        },
                        ["tuNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Từ ngày (yyyy-MM-dd). Để trống = tất cả."
                        },
                        ["denNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Đến ngày (yyyy-MM-dd). Để trống = đến hiện tại."
                        }
                    }
                },
                new ToolDefinition
                {
                    Name = "GetDeliveryScheduleByCustomer",
                    Description = "Lịch giao báo chi tiết của 1 khách hàng theo ngày: từng đầu báo, ngày nhận, SL phát, SL bán thực, SL dư. Dùng khi user hỏi 'lịch giao của KH X', 'KH X nhận báo gì hôm nay/tuần này', 'chi tiết phát báo KH X'.",
                    Parameters = new Dictionary<string, ToolParameter>
                    {
                        ["maKhachHang"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Mã khách hàng (MaKH) - bắt buộc nếu không có tenKhachHang."
                        },
                        ["tenKhachHang"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Tên khách hàng - dùng để tìm MaKH nếu không biết mã."
                        },
                        ["tuNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Từ ngày (yyyy-MM-dd). Để trống = 30 ngày gần nhất."
                        },
                        ["denNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Đến ngày (yyyy-MM-dd). Để trống = hôm nay."
                        },
                        ["topN"] = new ToolParameter
                        {
                            Type = "integer",
                            Description = "Số dòng chi tiết tối đa (mặc định 50, tối đa 500)."
                        }
                    }
                },
                new ToolDefinition
                {
                    Name = "GetChurnRiskCustomers",
                    Description = "Khách hàng có nguy cơ bỏ - tức là KH không mua hàng từ N ngày trước. Dùng khi user hỏi 'KH nào đang ngừng mua', 'KH nào lâu không lên đơn', 'rủi ro bỏ KH', 'KH ngủ đông'.",
                    Parameters = new Dictionary<string, ToolParameter>
                    {
                        ["soNgayKhongMua"] = new ToolParameter
                        {
                            Type = "integer",
                            Description = "Ngưỡng số ngày không mua (mặc định 90 ngày)."
                        },
                        ["topN"] = new ToolParameter
                        {
                            Type = "integer",
                            Description = "Số khách muốn xem (mặc định 20, tối đa 100)."
                        }
                    }
                },
                new ToolDefinition
                {
                    Name = "GetProfitMarginByPublication",
                    Description = "Lợi nhuận gộp ước tính theo từng đầu báo: doanh thu − chi phí ước tính (chi phí = SL bán × đơn giá × 0.85). Dùng khi user hỏi 'báo nào lãi nhất', 'lợi nhuận từng đầu báo', 'margin', 'biên lợi nhuận'.",
                    Parameters = new Dictionary<string, ToolParameter>
                    {
                        ["topN"] = new ToolParameter
                        {
                            Type = "integer",
                            Description = "Số đầu báo muốn xem (mặc định 10, tối đa 50)."
                        },
                        ["tuNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Từ ngày (yyyy-MM-dd). Để trống = tất cả."
                        },
                        ["denNgay"] = new ToolParameter
                        {
                            Type = "string",
                            Description = "Đến ngày (yyyy-MM-dd). Để trống = đến hiện tại."
                        }
                    }
                }
            };
        }

        public async Task<ToolResult> ExecuteAsync(ToolCallRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.ToolName))
                return new ToolResult { Success = false, Error = "Tool name is required." };

            try
            {
                return request.ToolName.ToLowerInvariant() switch
                {
                    "searchcustomers" => await SearchCustomersAsync(request.Parameters, cancellationToken),
                    "searchpublications" => await SearchPublicationsAsync(request.Parameters, cancellationToken),
                    "searchinvoices" => await SearchInvoicesAsync(request.Parameters, cancellationToken),
                    "getinventory" => await GetInventoryAsync(request.Parameters, cancellationToken),
                    "getdashboardsummary" => await GetDashboardSummaryAsync(request.Parameters, cancellationToken),
                    "gettopcustomersbyrevenue" => await GetTopCustomersByRevenueAsync(request.Parameters, cancellationToken),
                    "gettoppublicationsbyrevenue" => await GetTopPublicationsByRevenueAsync(request.Parameters, cancellationToken),
                    "getrevenuebyperiod" => await GetRevenueByPeriodAsync(request.Parameters, cancellationToken),
                    "getoverdueinvoices" => await GetOverdueInvoicesAsync(request.Parameters, cancellationToken),
                    "getlowinventory" => await GetLowInventoryAsync(request.Parameters, cancellationToken),
                    "getcustomeractivity" => await GetCustomerActivityAsync(request.Parameters, cancellationToken),
                    "getcustomerinventorywaste" => await GetCustomerInventoryWasteAsync(request.Parameters, cancellationToken),
                    "getpublicationsbywasterate" => await GetPublicationsByWasteRateAsync(request.Parameters, cancellationToken),
                    "getdeliveryschedulebycustomer" => await GetDeliveryScheduleByCustomerAsync(request.Parameters, cancellationToken),
                    "getchurnriskcustomers" => await GetChurnRiskCustomersAsync(request.Parameters, cancellationToken),
                    "getprofitmarginbypublication" => await GetProfitMarginByPublicationAsync(request.Parameters, cancellationToken),
                    _ => new ToolResult { Success = false, Error = $"Unknown tool: {request.ToolName}" }
                };
            }
            catch (OperationCanceledException)
            {
                return new ToolResult { Success = false, Error = "Tool execution cancelled." };
            }
            catch (Exception ex)
            {
                return new ToolResult { Success = false, Error = $"Tool error: {ex.Message}" };
            }
        }

        private async Task<ToolResult> SearchCustomersAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            parameters.TryGetValue("keyword", out var keywordObj);
            parameters.TryGetValue("loaiKh", out var loaiObj);
            string keyword = keywordObj?.ToString()?.Trim() ?? string.Empty;
            string loaiKh = loaiObj?.ToString()?.Trim() ?? string.Empty;

            string sql = @"SELECT MaKH, Ten, Diachi, Dienthoai, Chietkhau, Uutien, P_PH, P_KT
                           FROM TabKhachhang
                           WHERE 1=1";

            var cmdParams = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(loaiKh))
            {
                switch (loaiKh.ToUpperInvariant())
                {
                    case "P_PH":
                        sql += " AND P_PH = 1";
                        break;
                    case "P_KT":
                        sql += " AND P_KT = 1";
                        break;
                    case "P_PH_P_KT":
                        sql += " AND P_PH = 1 AND P_KT = 1";
                        break;
                }
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                sql += " AND (Ten LIKE @kw OR Dienthoai LIKE @kw OR MaKH LIKE @kw)";
                cmdParams.Add(new SqlParameter("@kw", SqlDbType.NVarChar) { Value = $"%{keyword}%" });
            }

            sql += " ORDER BY Ten";

            var dt = await DbHelper.Instance.FillDataTableAsync(sql, ct, cmdParams.ToArray());

            string loaiLabel = string.IsNullOrEmpty(loaiKh) ? "tất cả" : loaiKh;
            string countMsg = $"Có {dt.Rows.Count} khách hàng loại {loaiLabel}";

            if (dt.Rows.Count == 0)
            {
                string msg = string.IsNullOrEmpty(keyword)
                    ? $"{countMsg}."
                    : $"Không tìm thấy khách hàng nào phù hợp với từ khóa '{keyword}'.";
                return new ToolResult { Success = true, Output = msg };
            }

            var lines = new List<string> { $"{countMsg}:" };
            foreach (DataRow row in dt.Rows)
            {
                string ma = row["MaKH"]?.ToString() ?? "";
                string ten = row["Ten"]?.ToString() ?? "";
                string dtStr = row["Dienthoai"]?.ToString() ?? "";
                string ck = row["Chietkhau"] != DBNull.Value ? Convert.ToInt16(row["Chietkhau"]).ToString() + "%" : "0%";
                bool pph = row["P_PH"] != DBNull.Value && Convert.ToBoolean(row["P_PH"]);
                bool pkt = row["P_KT"] != DBNull.Value && Convert.ToBoolean(row["P_KT"]);
                string loai = pkt && pph ? "P_PH & P_KT" : pkt ? "P_KT" : "P_PH";

                lines.Add($"- [{ma}] {ten} | ĐT: {dtStr} | CK: {ck} | Loại: {loai}");
            }

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }

        private async Task<ToolResult> SearchPublicationsAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            parameters.TryGetValue("keyword", out var keywordObj);
            string keyword = keywordObj?.ToString()?.Trim() ?? string.Empty;

            string sql = @"SELECT MaBao, Ten, Dvt, DonGia, NgayBatDau,
                                  Thu1, Thu2, Thu3, Thu4, Thu5, Thu6, Thu7, SoLanPhtrongTuan, Sogoc
                           FROM TabBao
                           WHERE 1=1";

            var cmdParams = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(keyword))
            {
                sql += " AND Ten LIKE @kw";
                cmdParams.Add(new SqlParameter("@kw", SqlDbType.NVarChar) { Value = $"%{keyword}%" });
            }

            sql += " ORDER BY Ten";

            var dt = await DbHelper.Instance.FillDataTableAsync(sql, ct, cmdParams.ToArray());

            if (dt.Rows.Count == 0)
            {
                string msg = string.IsNullOrEmpty(keyword)
                    ? "Không có báo/tạp chí nào trong hệ thống."
                    : $"Không tìm thấy báo/tạp chí nào với từ khóa '{keyword}'.";
                return new ToolResult { Success = true, Output = msg };
            }

            var lines = new List<string> { $"Tìm thấy {dt.Rows.Count} báo/tạp chí:" };
            foreach (DataRow row in dt.Rows)
            {
                string maBao = row["MaBao"]?.ToString() ?? "";
                string ten = row["Ten"]?.ToString() ?? "";
                string dvt = row["Dvt"]?.ToString() ?? "";
                string donGia = row["DonGia"] != DBNull.Value
                    ? Convert.ToDouble(row["DonGia"]).ToString("N0") + "đ"
                    : "N/A";
                string ngayBD = row["NgayBatDau"] != DBNull.Value
                    ? ((DateTime)row["NgayBatDau"]).ToString("dd/MM/yyyy")
                    : "N/A";

                int pubDays = 0;
                for (int i = 1; i <= 7; i++)
                {
                    if (row[$"Thu{i}"] != DBNull.Value && (bool)row[$"Thu{i}"])
                        pubDays++;
                }
                string schedule = pubDays > 0 ? $"{pubDays} ngày/tuần" : "Không lịch";

                lines.Add($"- [{maBao}] {ten} | ĐVT: {dvt} | Đơn giá: {donGia} | Lịch: {schedule} | BĐ: {ngayBD}");
            }

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }

        private async Task<ToolResult> SearchInvoicesAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            parameters.TryGetValue("soHd", out var soHdObj);
            parameters.TryGetValue("tenKhachHang", out var tenKhObj);
            parameters.TryGetValue("ngay", out var ngayObj);

            string soHd = soHdObj?.ToString()?.Trim() ?? string.Empty;
            string tenKhachHang = tenKhObj?.ToString()?.Trim() ?? string.Empty;
            string ngay = ngayObj?.ToString()?.Trim() ?? string.Empty;

            string sql = @"SELECT hd.SoHD, hd.NgayLapPhieu, hd.TuNgay, hd.DenNgay,
                                  hd.ThanhToan, kh.MaKH, kh.Ten AS TenKhachHang, kh.Dienthoai
                           FROM TabHoadon hd
                           INNER JOIN TabKhachhang kh ON hd.MaKH = kh.MaKH
                           WHERE 1=1";

            var cmdParams = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(soHd))
            {
                sql += " AND hd.SoHD LIKE @sohd";
                cmdParams.Add(new SqlParameter("@sohd", SqlDbType.NVarChar) { Value = $"%{soHd}%" });
            }

            if (!string.IsNullOrEmpty(tenKhachHang))
            {
                sql += " AND (kh.Ten LIKE @ten OR kh.MaKH LIKE @ten)";
                cmdParams.Add(new SqlParameter("@ten", SqlDbType.NVarChar) { Value = $"%{tenKhachHang}%" });
            }

            if (!string.IsNullOrEmpty(ngay) && DateTime.TryParse(ngay, out var ngayDt))
            {
                sql += " AND CAST(hd.NgayLapPhieu AS DATE) = @ngay";
                cmdParams.Add(new SqlParameter("@ngay", SqlDbType.Date) { Value = ngayDt.Date });
            }

            sql += " ORDER BY hd.NgayLapPhieu DESC";

            var dt = await DbHelper.Instance.FillDataTableAsync(sql, ct, cmdParams.ToArray());

            if (dt.Rows.Count == 0)
            {
                string msg = "Không tìm thấy hóa đơn nào phù hợp.";
                return new ToolResult { Success = true, Output = msg };
            }

            var lines = new List<string> { $"Tìm thấy {dt.Rows.Count} hóa đơn:" };
            foreach (DataRow row in dt.Rows)
            {
                string sohd = row["SoHD"]?.ToString() ?? "";
                string ngayLap = row["NgayLapPhieu"] != DBNull.Value
                    ? ((DateTime)row["NgayLapPhieu"]).ToString("dd/MM/yyyy")
                    : "N/A";
                string tuNgay = row["TuNgay"] != DBNull.Value
                    ? ((DateTime)row["TuNgay"]).ToString("dd/MM/yyyy")
                    : "N/A";
                string denNgay = row["DenNgay"] != DBNull.Value
                    ? ((DateTime)row["DenNgay"]).ToString("dd/MM/yyyy")
                    : "N/A";
                string tenKH = row["TenKhachHang"]?.ToString() ?? "";
                string thanhToan = row["ThanhToan"] != DBNull.Value && (bool)row["ThanhToan"]
                    ? "Đã thanh toán"
                    : "Chưa thanh toán";

                lines.Add($"- [{sohd}] {tenKH} | Ngày lập: {ngayLap} | Từ {tuNgay} đến {denNgay} | {thanhToan}");
            }

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }

        private async Task<ToolResult> GetInventoryAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            parameters.TryGetValue("maBao", out var maBaoObj);
            parameters.TryGetValue("tenBao", out var tenBaoObj);

            string maBao = maBaoObj?.ToString()?.Trim() ?? string.Empty;
            string tenBao = tenBaoObj?.ToString()?.Trim() ?? string.Empty;

            string sql = @"SELECT t.Ngay, t.MaBao, b.Ten AS TenBao, t.SoBao, t.SlPhatHanh,
                                  t.Banthuc, t.BanLe, t.DieuPhoi, t.Ton
                           FROM TabTon t
                           INNER JOIN TabBao b ON t.MaBao = b.MaBao
                           WHERE 1=1";

            var cmdParams = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(maBao))
            {
                sql += " AND t.MaBao LIKE @mabao";
                cmdParams.Add(new SqlParameter("@mabao", SqlDbType.NVarChar) { Value = $"%{maBao}%" });
            }

            if (!string.IsNullOrEmpty(tenBao))
            {
                sql += " AND b.Ten LIKE @tenbao";
                cmdParams.Add(new SqlParameter("@tenbao", SqlDbType.NVarChar) { Value = $"%{tenBao}%" });
            }

            sql += " ORDER BY t.Ngay DESC, b.Ten";

            var dt = await DbHelper.Instance.FillDataTableAsync(sql, ct, cmdParams.ToArray());

            if (dt.Rows.Count == 0)
            {
                string msg = string.IsNullOrEmpty(maBao) && string.IsNullOrEmpty(tenBao)
                    ? "Không có dữ liệu tồn kho."
                    : "Không tìm thấy tồn kho nào phù hợp với bộ lọc.";
                return new ToolResult { Success = true, Output = msg };
            }

            var lines = new List<string> { $"Tồn kho ({dt.Rows.Count} bản ghi):" };
            foreach (DataRow row in dt.Rows)
            {
                string ngay = row["Ngay"] != DBNull.Value
                    ? ((DateTime)row["Ngay"]).ToString("dd/MM/yyyy")
                    : "N/A";
                string ten = row["TenBao"]?.ToString() ?? row["MaBao"]?.ToString() ?? "";
                string soBao = row["SoBao"] != DBNull.Value ? Convert.ToInt32(row["SoBao"]).ToString() : "-";
                string phatHanh = row["SlPhatHanh"] != DBNull.Value ? Convert.ToInt32(row["SlPhatHanh"]).ToString() : "0";
                string banThuc = row["Banthuc"] != DBNull.Value ? Convert.ToInt32(row["Banthuc"]).ToString() : "0";
                string banLe = row["BanLe"] != DBNull.Value ? Convert.ToInt32(row["BanLe"]).ToString() : "0";
                string dieuPhoi = row["DieuPhoi"] != DBNull.Value ? Convert.ToInt32(row["DieuPhoi"]).ToString() : "0";
                string ton = row["Ton"] != DBNull.Value ? Convert.ToInt32(row["Ton"]).ToString() : "0";

                lines.Add($"- [{ngay}] {ten}: Phát hành={phatHanh}, Bán thực={banThuc}, Bán lẻ={banLe}, Điều phối={dieuPhoi}, Tồn={ton}");
            }

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }

        private async Task<ToolResult> GetDashboardSummaryAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            string sqlDoanhThu = @"SELECT ISNULL(SUM(ct.SoLuongThuc * b.DonGia), 0)
                                    FROM TabHoadon hd
                                    INNER JOIN TabChiTietHoaDon ct ON hd.SoHD = ct.SoHD
                                    INNER JOIN TabBao b ON ct.MaBao = b.MaBao
                                    WHERE hd.ThanhToan = 1";

            decimal tongDoanhThu = await DbHelper.Instance.ExecuteScalarAsync<decimal>(sqlDoanhThu, ct);

            string sqlSoBao = "SELECT COUNT(*) FROM TabBao";
            int tongSoBao = await DbHelper.Instance.ExecuteScalarAsync<int>(sqlSoBao, ct);

            string sqlSoKH = "SELECT COUNT(*) FROM TabKhachhang";
            int tongKhachHang = await DbHelper.Instance.ExecuteScalarAsync<int>(sqlSoKH, ct);

            string sqlTyTrong = @"SELECT b.Ten, ISNULL(SUM(ct.SoLuongThuc * b.DonGia), 0) AS DoanhThu
                                  FROM TabChiTietHoaDon ct
                                  INNER JOIN TabBao b ON ct.MaBao = b.MaBao
                                  INNER JOIN TabHoadon hd ON ct.SoHD = hd.SoHD
                                  WHERE hd.ThanhToan = 1
                                  GROUP BY b.Ten
                                  ORDER BY DoanhThu DESC";

            var dtTyTrong = await DbHelper.Instance.FillDataTableAsync(sqlTyTrong, ct);

            var lines = new List<string>
            {
                $"Tổng doanh thu: {tongDoanhThu:N0}đ",
                $"Tổng số báo/tạp chí: {tongSoBao}",
                $"Tổng khách hàng: {tongKhachHang}",
                ""
            };

            if (dtTyTrong.Rows.Count > 0)
            {
                lines.Add("Tỷ trọng doanh thu theo báo:");
                foreach (DataRow row in dtTyTrong.Rows)
                {
                    string ten = row["Ten"]?.ToString() ?? "";
                    decimal dtVal = row["DoanhThu"] != DBNull.Value ? Convert.ToDecimal(row["DoanhThu"]) : 0;
                    string pct = tongDoanhThu > 0 ? $" ({(dtVal / tongDoanhThu * 100):F1}%)" : "";
                    lines.Add($"- {ten}: {dtVal:N0}đ{pct}");
                }
            }

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }

        private async Task<ToolResult> GetTopCustomersByRevenueAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            parameters.TryGetValue("topN", out var topNObj);
            parameters.TryGetValue("tuNgay", out var tuNgayObj);
            parameters.TryGetValue("denNgay", out var denNgayObj);

            int topN = 10;
            if (topNObj != null && int.TryParse(topNObj.ToString(), out var n) && n > 0)
                topN = Math.Min(n, 50);

            string tuNgay = tuNgayObj?.ToString()?.Trim() ?? string.Empty;
            string denNgay = denNgayObj?.ToString()?.Trim() ?? string.Empty;

            string sql = @"SELECT TOP (@topN) kh.MaKH, kh.Ten, kh.Dienthoai,
                                  COUNT(DISTINCT hd.SoHD) AS SoHoaDon,
                                  ISNULL(SUM(ct.ThanhTien), 0) AS TongDoanhThu
                           FROM TabKhachhang kh
                           INNER JOIN TabHoadon hd ON kh.MaKH = hd.MaKH
                           INNER JOIN TabChiTietHoaDon ct ON hd.SoHD = ct.SoHD
                           WHERE hd.ThanhToan = 1";

            var ps = new List<SqlParameter>
            {
                new SqlParameter("@topN", SqlDbType.Int) { Value = topN }
            };

            DateTime tuNgayDt, denNgayDt;
            bool hasTu = DateTime.TryParse(tuNgay, out tuNgayDt);
            bool hasDen = DateTime.TryParse(denNgay, out denNgayDt);

            if (hasTu)
            {
                sql += " AND CAST(hd.NgayLapPhieu AS DATE) >= @tu";
                ps.Add(new SqlParameter("@tu", SqlDbType.Date) { Value = tuNgayDt.Date });
            }
            if (hasDen)
            {
                sql += " AND CAST(hd.NgayLapPhieu AS DATE) <= @den";
                ps.Add(new SqlParameter("@den", SqlDbType.Date) { Value = denNgayDt.Date });
            }

            sql += @" GROUP BY kh.MaKH, kh.Ten, kh.Dienthoai
                      ORDER BY TongDoanhThu DESC";

            var dt = await DbHelper.Instance.FillDataTableAsync(sql, ct, ps.ToArray());

            if (dt.Rows.Count == 0)
                return new ToolResult { Success = true, Output = "Không có dữ liệu doanh thu khách hàng trong khoảng thời gian này." };

            var lines = new List<string> { $"Top {dt.Rows.Count} khách hàng theo doanh thu:" };
            int stt = 1;
            foreach (DataRow row in dt.Rows)
            {
                string ma = row["MaKH"]?.ToString() ?? "";
                string ten = row["Ten"]?.ToString() ?? "";
                string dtStr = row["Dienthoai"]?.ToString() ?? "";
                int soHD = row["SoHoaDon"] != DBNull.Value ? Convert.ToInt32(row["SoHoaDon"]) : 0;
                decimal tong = row["TongDoanhThu"] != DBNull.Value ? Convert.ToDecimal(row["TongDoanhThu"]) : 0;
                lines.Add($"#{stt++} [{ma}] {ten} | ĐT: {dtStr} | Số HĐ: {soHD} | Tổng DT: {tong:N0}đ");
            }

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }

        private async Task<ToolResult> GetTopPublicationsByRevenueAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            parameters.TryGetValue("topN", out var topNObj);
            parameters.TryGetValue("tuNgay", out var tuNgayObj);
            parameters.TryGetValue("denNgay", out var denNgayObj);
            parameters.TryGetValue("theoSoLuong", out var theoSLObj);

            int topN = 10;
            if (topNObj != null && int.TryParse(topNObj.ToString(), out var n) && n > 0)
                topN = Math.Min(n, 50);

            string tuNgay = tuNgayObj?.ToString()?.Trim() ?? string.Empty;
            string denNgay = denNgayObj?.ToString()?.Trim() ?? string.Empty;
            bool theoSoLuong = theoSLObj != null
                && theoSLObj.ToString().Equals("true", StringComparison.OrdinalIgnoreCase);

            string valueExpr = theoSoLuong
                ? "ISNULL(SUM(ct.SoLuongThuc), 0)"
                : "ISNULL(SUM(ct.ThanhTien), 0)";
            string header = theoSoLuong ? "Tổng SL bán" : "Tổng doanh thu";

            string sql = $@"SELECT TOP (@topN) b.MaBao, b.Ten,
                                   {valueExpr} AS GiaTri
                            FROM TabBao b
                            INNER JOIN TabChiTietHoaDon ct ON b.MaBao = ct.MaBao
                            INNER JOIN TabHoadon hd ON ct.SoHD = hd.SoHD
                            WHERE hd.ThanhToan = 1";

            var ps = new List<SqlParameter>
            {
                new SqlParameter("@topN", SqlDbType.Int) { Value = topN }
            };

            DateTime tuNgayDt, denNgayDt;
            bool hasTu = DateTime.TryParse(tuNgay, out tuNgayDt);
            bool hasDen = DateTime.TryParse(denNgay, out denNgayDt);

            if (hasTu)
            {
                sql += " AND CAST(hd.NgayLapPhieu AS DATE) >= @tu";
                ps.Add(new SqlParameter("@tu", SqlDbType.Date) { Value = tuNgayDt.Date });
            }
            if (hasDen)
            {
                sql += " AND CAST(hd.NgayLapPhieu AS DATE) <= @den";
                ps.Add(new SqlParameter("@den", SqlDbType.Date) { Value = denNgayDt.Date });
            }

            sql += @" GROUP BY b.MaBao, b.Ten
                      ORDER BY GiaTri DESC";

            var dt = await DbHelper.Instance.FillDataTableAsync(sql, ct, ps.ToArray());

            if (dt.Rows.Count == 0)
                return new ToolResult { Success = true, Output = "Không có dữ liệu bán hàng trong khoảng thời gian này." };

            var lines = new List<string> { $"Top {dt.Rows.Count} báo/tạp chí ({header}):" };
            int stt = 1;
            foreach (DataRow row in dt.Rows)
            {
                string ma = row["MaBao"]?.ToString() ?? "";
                string ten = row["Ten"]?.ToString() ?? "";
                decimal giaTri = row["GiaTri"] != DBNull.Value ? Convert.ToDecimal(row["GiaTri"]) : 0;
                string formatted = theoSoLuong ? $"{giaTri:N0} cuốn" : $"{giaTri:N0}đ";
                lines.Add($"#{stt++} [{ma}] {ten} | {header}: {formatted}");
            }

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }

        private async Task<ToolResult> GetRevenueByPeriodAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            parameters.TryGetValue("tuNgay", out var tuNgayObj);
            parameters.TryGetValue("denNgay", out var denNgayObj);
            parameters.TryGetValue("groupBy", out var groupByObj);

            string tuNgay = tuNgayObj?.ToString()?.Trim() ?? string.Empty;
            string denNgay = denNgayObj?.ToString()?.Trim() ?? string.Empty;
            string groupBy = (groupByObj?.ToString() ?? "day").Trim().ToLowerInvariant();
            if (groupBy != "day" && groupBy != "month" && groupBy != "year")
                groupBy = "day";

            if (string.IsNullOrEmpty(tuNgay) || string.IsNullOrEmpty(denNgay)
                || !DateTime.TryParse(tuNgay, out var tuNgayDt)
                || !DateTime.TryParse(denNgay, out var denNgayDt))
            {
                return new ToolResult
                {
                    Success = false,
                    Error = "Cần cung cấp tuNgay và denNgay hợp lệ (định dạng yyyy-MM-dd)."
                };
            }

            if (tuNgayDt > denNgayDt)
                return new ToolResult { Success = false, Error = "tuNgay phải nhỏ hơn hoặc bằng denNgay." };

            int days = (denNgayDt.Date - tuNgayDt.Date).Days;
            if (groupBy == "day" && days > 366)
            {
                groupBy = "month";
            }
            if (groupBy == "month" && days > 365 * 5)
            {
                groupBy = "year";
            }

            string groupExpr = groupBy switch
            {
                "month" => "FORMAT(hd.NgayLapPhieu, 'yyyy-MM')",
                "year" => "FORMAT(hd.NgayLapPhieu, 'yyyy')",
                _ => "FORMAT(hd.NgayLapPhieu, 'yyyy-MM-dd')"
            };
            string groupLabel = groupBy switch
            {
                "month" => "Tháng",
                "year" => "Năm",
                _ => "Ngày"
            };

            string sql = $@"SELECT {groupExpr} AS Ky,
                                   ISNULL(SUM(ct.ThanhTien), 0) AS DoanhThu,
                                   COUNT(DISTINCT hd.SoHD) AS SoHoaDon
                            FROM TabHoadon hd
                            INNER JOIN TabChiTietHoaDon ct ON hd.SoHD = ct.SoHD
                            WHERE hd.ThanhToan = 1
                              AND CAST(hd.NgayLapPhieu AS DATE) >= @tu
                              AND CAST(hd.NgayLapPhieu AS DATE) <= @den
                            GROUP BY {groupExpr}
                            ORDER BY Ky";

            var ps = new List<SqlParameter>
            {
                new SqlParameter("@tu", SqlDbType.Date) { Value = tuNgayDt.Date },
                new SqlParameter("@den", SqlDbType.Date) { Value = denNgayDt.Date }
            };

            var dt = await DbHelper.Instance.FillDataTableAsync(sql, ct, ps.ToArray());

            if (dt.Rows.Count == 0)
                return new ToolResult
                {
                    Success = true,
                    Output = $"Không có doanh thu từ {tuNgay} đến {denNgay}."
                };

            decimal tong = 0;
            int tongHD = 0;
            var lines = new List<string>
            {
                $"Báo cáo doanh thu từ {tuNgayDt:dd/MM/yyyy} đến {denNgayDt:dd/MM/yyyy} (gom nhóm theo {groupLabel.ToLower()}):"
            };

            foreach (DataRow row in dt.Rows)
            {
                string ky = row["Ky"]?.ToString() ?? "";
                decimal doanhThu = row["DoanhThu"] != DBNull.Value ? Convert.ToDecimal(row["DoanhThu"]) : 0;
                int soHD = row["SoHoaDon"] != DBNull.Value ? Convert.ToInt32(row["SoHoaDon"]) : 0;
                tong += doanhThu;
                tongHD += soHD;
                lines.Add($"- {ky}: {doanhThu:N0}đ ({soHD} hóa đơn)");
            }

            lines.Add("");
            lines.Add($"Tổng: {tong:N0}đ từ {tongHD} hóa đơn.");

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }

        private async Task<ToolResult> GetOverdueInvoicesAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            parameters.TryGetValue("topN", out var topNObj);
            parameters.TryGetValue("chinhXacQuaHan", out var quaHanObj);

            int topN = 20;
            if (topNObj != null && int.TryParse(topNObj.ToString(), out var n) && n > 0)
                topN = Math.Min(n, 100);

            bool quaHan = quaHanObj != null
                && quaHanObj.ToString().Equals("true", StringComparison.OrdinalIgnoreCase);

            string sql = @"SELECT TOP (@topN) hd.SoHD, kh.MaKH, kh.Ten, kh.Dienthoai,
                                  hd.NgayLapPhieu, hd.TuNgay, hd.DenNgay,
                                  ISNULL(SUM(ct.ThanhTien), 0) AS TongTien
                           FROM TabHoadon hd
                           INNER JOIN TabKhachhang kh ON hd.MaKH = kh.MaKH
                           LEFT JOIN TabChiTietHoaDon ct ON hd.SoHD = ct.SoHD
                           WHERE hd.ThanhToan = 0";

            if (quaHan)
            {
                sql += " AND hd.DenNgay < CAST(GETDATE() AS DATE)";
            }

            sql += @" GROUP BY hd.SoHD, kh.MaKH, kh.Ten, kh.Dienthoai,
                                 hd.NgayLapPhieu, hd.TuNgay, hd.DenNgay
               ORDER BY hd.DenNgay ASC";

            var ps = new List<SqlParameter>
            {
                new SqlParameter("@topN", SqlDbType.Int) { Value = topN }
            };

            var dt = await DbHelper.Instance.FillDataTableAsync(sql, ct, ps.ToArray());

            if (dt.Rows.Count == 0)
                return new ToolResult
                {
                    Success = true,
                    Output = quaHan
                        ? "Không có hóa đơn quá hạn nào."
                        : "Không có hóa đơn chưa thanh toán nào."
                };

            var lines = new List<string>
            {
                $"Tìm thấy {dt.Rows.Count} hóa đơn {(quaHan ? "quá hạn" : "chưa thanh toán")}:"
            };

            decimal tongCongNo = 0;
            foreach (DataRow row in dt.Rows)
            {
                string sohd = row["SoHD"]?.ToString() ?? "";
                string maKH = row["MaKH"]?.ToString() ?? "";
                string tenKH = row["Ten"]?.ToString() ?? "";
                string dtStr = row["Dienthoai"]?.ToString() ?? "";
                string ngayLap = row["NgayLapPhieu"] != DBNull.Value
                    ? ((DateTime)row["NgayLapPhieu"]).ToString("dd/MM/yyyy")
                    : "N/A";
                string denNgay = row["DenNgay"] != DBNull.Value
                    ? ((DateTime)row["DenNgay"]).ToString("dd/MM/yyyy")
                    : "N/A";
                decimal tong = row["TongTien"] != DBNull.Value ? Convert.ToDecimal(row["TongTien"]) : 0;
                tongCongNo += tong;

                string quaHanTag = "";
                if (quaHan && row["DenNgay"] != DBNull.Value)
                {
                    int soNgayQuaHan = (int)(DateTime.Today - ((DateTime)row["DenNgay"]).Date).TotalDays;
                    if (soNgayQuaHan > 0)
                        quaHanTag = $" (quá {soNgayQuaHan} ngày)";
                }

                lines.Add($"- [{sohd}] {tenKH} ({maKH}) | ĐT: {dtStr} | Lập: {ngayLap} | Hạn: {denNgay}{quaHanTag} | Nợ: {tong:N0}đ");
            }

            lines.Add("");
            lines.Add($"Tổng công nợ: {tongCongNo:N0}đ từ {dt.Rows.Count} hóa đơn.");

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }

        private async Task<ToolResult> GetLowInventoryAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            parameters.TryGetValue("nguongTon", out var nguongObj);
            parameters.TryGetValue("ngay", out var ngayObj);

            int nguongTon = 10;
            if (nguongObj != null && int.TryParse(nguongObj.ToString(), out var n) && n >= 0)
                nguongTon = n;

            string ngay = ngayObj?.ToString()?.Trim() ?? string.Empty;

            string sql;
            var ps = new List<SqlParameter>
            {
                new SqlParameter("@nguong", SqlDbType.Int) { Value = nguongTon }
            };

            if (!string.IsNullOrEmpty(ngay) && DateTime.TryParse(ngay, out var ngayDt))
            {
                sql = @"SELECT t.Ngay, t.MaBao, b.Ten AS TenBao, t.Ton, t.SlPhatHanh,
                               t.Banthuc, t.BanLe, t.DieuPhoi
                        FROM TabTon t
                        INNER JOIN TabBao b ON t.MaBao = b.MaBao
                        WHERE CAST(t.Ngay AS DATE) = @ngay
                          AND ISNULL(t.Ton, 0) <= @nguong
                        ORDER BY t.Ton ASC, b.Ten";
                ps.Add(new SqlParameter("@ngay", SqlDbType.Date) { Value = ngayDt.Date });
            }
            else
            {
                sql = @"SELECT t.Ngay, t.MaBao, b.Ten AS TenBao, t.Ton, t.SlPhatHanh,
                               t.Banthuc, t.BanLe, t.DieuPhoi
                        FROM TabTon t
                        INNER JOIN TabBao b ON t.MaBao = b.MaBao
                        WHERE t.Ngay = (SELECT MAX(Ngay) FROM TabTon)
                          AND ISNULL(t.Ton, 0) <= @nguong
                        ORDER BY t.Ton ASC, b.Ten";
            }

            var dt = await DbHelper.Instance.FillDataTableAsync(sql, ct, ps.ToArray());

            if (dt.Rows.Count == 0)
                return new ToolResult
                {
                    Success = true,
                    Output = $"Không có đầu báo nào có tồn kho <= {nguongTon}."
                };

            var lines = new List<string>
            {
                $"Cảnh báo tồn kho thấp (<= {nguongTon}) — tìm thấy {dt.Rows.Count} đầu báo:"
            };
            foreach (DataRow row in dt.Rows)
            {
                string ngayStr = row["Ngay"] != DBNull.Value
                    ? ((DateTime)row["Ngay"]).ToString("dd/MM/yyyy")
                    : "N/A";
                string ma = row["MaBao"]?.ToString() ?? "";
                string ten = row["TenBao"]?.ToString() ?? "";
                int ton = row["Ton"] != DBNull.Value ? Convert.ToInt32(row["Ton"]) : 0;
                int phatHanh = row["SlPhatHanh"] != DBNull.Value ? Convert.ToInt32(row["SlPhatHanh"]) : 0;
                int banThuc = row["Banthuc"] != DBNull.Value ? Convert.ToInt32(row["Banthuc"]) : 0;
                lines.Add($"- [{ma}] {ten} | Ngày: {ngayStr} | Tồn: {ton} (PH={phatHanh}, bán={banThuc})");
            }

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }

        private async Task<ToolResult> GetCustomerActivityAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            parameters.TryGetValue("maKhachHang", out var maObj);
            parameters.TryGetValue("tenKhachHang", out var tenObj);
            parameters.TryGetValue("tuNgay", out var tuNgayObj);
            parameters.TryGetValue("denNgay", out var denNgayObj);

            string maKH = maObj?.ToString()?.Trim() ?? string.Empty;
            string tenKH = tenObj?.ToString()?.Trim() ?? string.Empty;
            string tuNgay = tuNgayObj?.ToString()?.Trim() ?? string.Empty;
            string denNgay = denNgayObj?.ToString()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(maKH) && string.IsNullOrEmpty(tenKH))
            {
                return new ToolResult
                {
                    Success = false,
                    Error = "Cần cung cấp maKhachHang hoặc tenKhachHang."
                };
            }

            DataTable dtKh;
            var psKH = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(maKH))
            {
                string sqlKH = @"SELECT MaKH, Ten, Diachi, Dienthoai, Chietkhau
                                 FROM TabKhachhang WHERE MaKH = @ma";
                psKH.Add(new SqlParameter("@ma", SqlDbType.NVarChar) { Value = maKH });
                dtKh = await DbHelper.Instance.FillDataTableAsync(sqlKH, ct, psKH.ToArray());
            }
            else
            {
                string sqlKH = @"SELECT TOP 1 MaKH, Ten, Diachi, Dienthoai, Chietkhau
                                 FROM TabKhachhang WHERE Ten LIKE @ten";
                psKH.Add(new SqlParameter("@ten", SqlDbType.NVarChar) { Value = $"%{tenKH}%" });
                dtKh = await DbHelper.Instance.FillDataTableAsync(sqlKH, ct, psKH.ToArray());
            }

            if (dtKh.Rows.Count == 0)
            {
                return new ToolResult
                {
                    Success = true,
                    Output = $"Không tìm thấy khách hàng '{maKH}{tenKH}'."
                };
            }

            DataRow khRow = dtKh.Rows[0];
            maKH = khRow["MaKH"]?.ToString() ?? "";
            string ten = khRow["Ten"]?.ToString() ?? "";
            string diaChi = khRow["Diachi"]?.ToString() ?? "";
            string dThoai = khRow["Dienthoai"]?.ToString() ?? "";
            string chietKhau = khRow["Chietkhau"] != DBNull.Value
                ? Convert.ToInt16(khRow["Chietkhau"]) + "%" : "0%";

            string sqlHD = @"SELECT COUNT(*) AS SoHD,
                                    ISNULL(SUM(CASE WHEN hd.ThanhToan = 1 THEN ct.ThanhTien ELSE 0 END), 0) AS TongDaThanhToan,
                                    ISNULL(SUM(CASE WHEN hd.ThanhToan = 0 THEN ct.ThanhTien ELSE 0 END), 0) AS TongChuaThanhToan,
                                    MIN(hd.NgayLapPhieu) AS NgayMuaDau,
                                    MAX(hd.NgayLapPhieu) AS NgayMuaCuoi
                             FROM TabHoadon hd
                             LEFT JOIN TabChiTietHoaDon ct ON hd.SoHD = ct.SoHD
                             WHERE hd.MaKH = @ma";

            var psHD = new List<SqlParameter>
            {
                new SqlParameter("@ma", SqlDbType.NVarChar) { Value = maKH }
            };
            DateTime tuNgayDt, denNgayDt;
            bool hasTu = DateTime.TryParse(tuNgay, out tuNgayDt);
            bool hasDen = DateTime.TryParse(denNgay, out denNgayDt);
            if (hasTu)
            {
                sqlHD += " AND CAST(hd.NgayLapPhieu AS DATE) >= @tu";
                psHD.Add(new SqlParameter("@tu", SqlDbType.Date) { Value = tuNgayDt.Date });
            }
            if (hasDen)
            {
                sqlHD += " AND CAST(hd.NgayLapPhieu AS DATE) <= @den";
                psHD.Add(new SqlParameter("@den", SqlDbType.Date) { Value = denNgayDt.Date });
            }

            var dtHD = await DbHelper.Instance.FillDataTableAsync(sqlHD, ct, psHD.ToArray());
            int soHD = 0;
            decimal tongDaTT = 0, tongChuaTT = 0;
            string ngayDau = "N/A", ngayCuoi = "N/A";
            if (dtHD.Rows.Count > 0)
            {
                DataRow r = dtHD.Rows[0];
                soHD = r["SoHD"] != DBNull.Value ? Convert.ToInt32(r["SoHD"]) : 0;
                tongDaTT = r["TongDaThanhToan"] != DBNull.Value ? Convert.ToDecimal(r["TongDaThanhToan"]) : 0;
                tongChuaTT = r["TongChuaThanhToan"] != DBNull.Value ? Convert.ToDecimal(r["TongChuaThanhToan"]) : 0;
                ngayDau = r["NgayMuaDau"] != DBNull.Value ? ((DateTime)r["NgayMuaDau"]).ToString("dd/MM/yyyy") : "N/A";
                ngayCuoi = r["NgayMuaCuoi"] != DBNull.Value ? ((DateTime)r["NgayMuaCuoi"]).ToString("dd/MM/yyyy") : "N/A";
            }

            string sqlTopBao = @"SELECT TOP 5 b.MaBao, b.Ten,
                                        ISNULL(SUM(ct.ThanhTien), 0) AS DoanhThu,
                                        ISNULL(SUM(ct.SoLuongThuc), 0) AS TongSoLuong
                                 FROM TabChiTietHoaDon ct
                                 INNER JOIN TabHoadon hd ON ct.SoHD = hd.SoHD
                                 INNER JOIN TabBao b ON ct.MaBao = b.MaBao
                                 WHERE hd.MaKH = @ma";
            var psTop = new List<SqlParameter>
            {
                new SqlParameter("@ma", SqlDbType.NVarChar) { Value = maKH }
            };
            if (hasTu)
            {
                sqlTopBao += " AND CAST(hd.NgayLapPhieu AS DATE) >= @tu";
                psTop.Add(new SqlParameter("@tu", SqlDbType.Date) { Value = tuNgayDt.Date });
            }
            if (hasDen)
            {
                sqlTopBao += " AND CAST(hd.NgayLapPhieu AS DATE) <= @den";
                psTop.Add(new SqlParameter("@den", SqlDbType.Date) { Value = denNgayDt.Date });
            }
            sqlTopBao += @" GROUP BY b.MaBao, b.Ten
                            ORDER BY DoanhThu DESC";

            var dtTop = await DbHelper.Instance.FillDataTableAsync(sqlTopBao, ct, psTop.ToArray());

            var lines = new List<string>
            {
                $"Hoạt động mua hàng của: {ten} [{maKH}]",
                $"ĐT: {dThoai} | ĐC: {diaChi} | CK: {chietKhau}",
                "",
                $"Số hóa đơn: {soHD}",
                $"Tổng đã thanh toán: {tongDaTT:N0}đ",
                $"Tổng chưa thanh toán: {tongChuaTT:N0}đ",
                $"Mua đầu tiên: {ngayDau} | Gần nhất: {ngayCuoi}"
            };

            if (dtTop.Rows.Count > 0)
            {
                lines.Add("");
                lines.Add("Top 5 báo/tạp chí đã mua:");
                int stt = 1;
                foreach (DataRow r in dtTop.Rows)
                {
                    string ma = r["MaBao"]?.ToString() ?? "";
                    string tenB = r["Ten"]?.ToString() ?? "";
                    decimal dtVal = r["DoanhThu"] != DBNull.Value ? Convert.ToDecimal(r["DoanhThu"]) : 0;
                    decimal sl = r["TongSoLuong"] != DBNull.Value ? Convert.ToDecimal(r["TongSoLuong"]) : 0;
                    lines.Add($"  #{stt++} [{ma}] {tenB}: {dtVal:N0}đ ({sl:N0} cuốn)");
                }
            }

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }

        private async Task<ToolResult> GetCustomerInventoryWasteAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            parameters.TryGetValue("topN", out var topNObj);
            parameters.TryGetValue("tuNgay", out var tuNgayObj);
            parameters.TryGetValue("denNgay", out var denNgayObj);

            int topN = 10;
            if (topNObj != null && int.TryParse(topNObj.ToString(), out var n) && n > 0)
                topN = Math.Min(n, 50);

            string tuNgay = tuNgayObj?.ToString()?.Trim() ?? string.Empty;
            string denNgay = denNgayObj?.ToString()?.Trim() ?? string.Empty;

            string sql = @"SELECT TOP (@topN) kh.MaKH, kh.Ten, kh.Dienthoai,
                                  ISNULL(SUM(ct.SoLuongDu), 0) AS TongSLDu,
                                  ISNULL(SUM(ct.SoLuongThuc), 0) AS TongSLBan,
                                  ISNULL(SUM(ct.ThanhTien * ct.SoLuongDu / NULLIF(ct.SoLuongDu + ct.SoLuongThuc, 0)), 0) AS TienMatDoTon,
                                  COUNT(DISTINCT hd.SoHD) AS SoHoaDon
                           FROM TabKhachhang kh
                           INNER JOIN TabHoadon hd ON kh.MaKH = hd.MaKH
                           INNER JOIN TabChiTietHoaDon ct ON hd.SoHD = ct.SoHD
                           WHERE ct.SoLuongDu > 0";

            var ps = new List<SqlParameter>
            {
                new SqlParameter("@topN", SqlDbType.Int) { Value = topN }
            };

            DateTime tuNgayDt, denNgayDt;
            bool hasTu = DateTime.TryParse(tuNgay, out tuNgayDt);
            bool hasDen = DateTime.TryParse(denNgay, out denNgayDt);

            if (hasTu)
            {
                sql += " AND CAST(hd.NgayLapPhieu AS DATE) >= @tu";
                ps.Add(new SqlParameter("@tu", SqlDbType.Date) { Value = tuNgayDt.Date });
            }
            if (hasDen)
            {
                sql += " AND CAST(hd.NgayLapPhieu AS DATE) <= @den";
                ps.Add(new SqlParameter("@den", SqlDbType.Date) { Value = denNgayDt.Date });
            }

            sql += @" GROUP BY kh.MaKH, kh.Ten, kh.Dienthoai
                      ORDER BY TongSLDu DESC";

            var dt = await DbHelper.Instance.FillDataTableAsync(sql, ct, ps.ToArray());

            if (dt.Rows.Count == 0)
                return new ToolResult { Success = true, Output = "Không có khách hàng nào có số lượng dư (tồn) trong khoảng thời gian này." };

            var lines = new List<string> { $"Top {dt.Rows.Count} khách hàng có số lượng dư (tồn) nhiều nhất:" };
            int stt = 1;
            foreach (DataRow row in dt.Rows)
            {
                string ma = row["MaKH"]?.ToString() ?? "";
                string ten = row["Ten"]?.ToString() ?? "";
                string dienthoai = row["Dienthoai"]?.ToString() ?? "";
                decimal slDu = row["TongSLDu"] != DBNull.Value ? Convert.ToDecimal(row["TongSLDu"]) : 0;
                decimal slBan = row["TongSLBan"] != DBNull.Value ? Convert.ToDecimal(row["TongSLBan"]) : 0;
                decimal tienMat = row["TienMatDoTon"] != DBNull.Value ? Convert.ToDecimal(row["TienMatDoTon"]) : 0;
                int soHD = row["SoHoaDon"] != DBNull.Value ? Convert.ToInt32(row["SoHoaDon"]) : 0;
                decimal tiLeTon = (slDu + slBan) > 0 ? Math.Round(slDu * 100m / (slDu + slBan), 2) : 0;
                lines.Add($"#{stt++} [{ma}] {ten} | ĐT: {dienthoai} | SL dư: {slDu:N0} | SL bán: {slBan:N0} | Tỷ lệ tồn: {tiLeTon}% | Tiền mất ~{tienMat:N0}đ | Số HĐ: {soHD}");
            }

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }

        private async Task<ToolResult> GetPublicationsByWasteRateAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            parameters.TryGetValue("topN", out var topNObj);
            parameters.TryGetValue("tuNgay", out var tuNgayObj);
            parameters.TryGetValue("denNgay", out var denNgayObj);

            int topN = 10;
            if (topNObj != null && int.TryParse(topNObj.ToString(), out var n) && n > 0)
                topN = Math.Min(n, 50);

            string tuNgay = tuNgayObj?.ToString()?.Trim() ?? string.Empty;
            string denNgay = denNgayObj?.ToString()?.Trim() ?? string.Empty;

            string sql = @"SELECT TOP (@topN) b.MaBao, b.Ten,
                                  ISNULL(SUM(ct.SoLuongDu), 0) AS TongSLDu,
                                  ISNULL(SUM(ct.SoLuongThuc), 0) AS TongSLBan,
                                  ISNULL(SUM(ct.SoLuongDu) * 1.0 / NULLIF(SUM(ct.SoLuongDu + ct.SoLuongThuc), 0), 0) AS WasteRate,
                                  ISNULL(SUM(ct.ThanhTien * ct.SoLuongDu / NULLIF(ct.SoLuongDu + ct.SoLuongThuc, 0)), 0) AS TienMat
                           FROM TabBao b
                           INNER JOIN TabChiTietHoaDon ct ON b.MaBao = ct.MaBao
                           INNER JOIN TabHoadon hd ON ct.SoHD = hd.SoHD
                           WHERE 1=1";

            var ps = new List<SqlParameter>
            {
                new SqlParameter("@topN", SqlDbType.Int) { Value = topN }
            };

            DateTime tuNgayDt, denNgayDt;
            bool hasTu = DateTime.TryParse(tuNgay, out tuNgayDt);
            bool hasDen = DateTime.TryParse(denNgay, out denNgayDt);

            if (hasTu)
            {
                sql += " AND CAST(hd.NgayLapPhieu AS DATE) >= @tu";
                ps.Add(new SqlParameter("@tu", SqlDbType.Date) { Value = tuNgayDt.Date });
            }
            if (hasDen)
            {
                sql += " AND CAST(hd.NgayLapPhieu AS DATE) <= @den";
                ps.Add(new SqlParameter("@den", SqlDbType.Date) { Value = denNgayDt.Date });
            }

            sql += @" GROUP BY b.MaBao, b.Ten
                      HAVING SUM(ct.SoLuongDu) > 0
                      ORDER BY WasteRate DESC";

            var dt = await DbHelper.Instance.FillDataTableAsync(sql, ct, ps.ToArray());

            if (dt.Rows.Count == 0)
                return new ToolResult { Success = true, Output = "Không có dữ liệu tồn theo đầu báo." };

            var lines = new List<string> { $"Top {dt.Rows.Count} đầu báo có tỷ lệ tồn (SL dư / tổng SL phát) cao nhất:" };
            int stt = 1;
            foreach (DataRow row in dt.Rows)
            {
                string ma = row["MaBao"]?.ToString() ?? "";
                string ten = row["Ten"]?.ToString() ?? "";
                decimal slDu = row["TongSLDu"] != DBNull.Value ? Convert.ToDecimal(row["TongSLDu"]) : 0;
                decimal slBan = row["TongSLBan"] != DBNull.Value ? Convert.ToDecimal(row["TongSLBan"]) : 0;
                decimal wasteRate = row["WasteRate"] != DBNull.Value ? Convert.ToDecimal(row["WasteRate"]) : 0;
                decimal tienMat = row["TienMat"] != DBNull.Value ? Convert.ToDecimal(row["TienMat"]) : 0;
                lines.Add($"#{stt++} [{ma}] {ten} | SL dư: {slDu:N0} | SL bán: {slBan:N0} | Tỷ lệ tồn: {wasteRate * 100:N2}% | Tiền mất ~{tienMat:N0}đ");
            }

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }

        private async Task<ToolResult> GetDeliveryScheduleByCustomerAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            parameters.TryGetValue("maKhachHang", out var maKhObj);
            parameters.TryGetValue("tenKhachHang", out var tenKhObj);
            parameters.TryGetValue("tuNgay", out var tuNgayObj);
            parameters.TryGetValue("denNgay", out var denNgayObj);
            parameters.TryGetValue("topN", out var topNObj);

            string maKh = maKhObj?.ToString()?.Trim() ?? string.Empty;
            string tenKh = tenKhObj?.ToString()?.Trim() ?? string.Empty;
            string tuNgay = tuNgayObj?.ToString()?.Trim() ?? string.Empty;
            string denNgay = denNgayObj?.ToString()?.Trim() ?? string.Empty;

            int topN = 50;
            if (topNObj != null && int.TryParse(topNObj.ToString(), out var n) && n > 0)
                topN = Math.Min(n, 500);

            if (string.IsNullOrEmpty(maKh) && !string.IsNullOrEmpty(tenKh))
            {
                string findSql = "SELECT TOP 1 MaKH, Ten FROM TabKhachhang WHERE Ten LIKE @kw ORDER BY Ten";
                var dtFind = await DbHelper.Instance.FillDataTableAsync(findSql, ct,
                    new[] { new SqlParameter("@kw", SqlDbType.NVarChar) { Value = $"%{tenKh}%" } });
                if (dtFind.Rows.Count > 0)
                {
                    maKh = dtFind.Rows[0]["MaKH"]?.ToString() ?? "";
                }
            }

            if (string.IsNullOrEmpty(maKh))
                return new ToolResult { Success = false, Error = "Cần cung cấp maKhachHang (mã KH) hoặc tenKhachHang để tra cứu." };

            DateTime tuNgayDt, denNgayDt;
            if (string.IsNullOrEmpty(tuNgay) || !DateTime.TryParse(tuNgay, out tuNgayDt))
                tuNgayDt = DateTime.Today.AddDays(-30);
            if (string.IsNullOrEmpty(denNgay) || !DateTime.TryParse(denNgay, out denNgayDt))
                denNgayDt = DateTime.Today;

            string sql = @"SELECT TOP (@topN) kh.MaKH, kh.Ten, kh.Dienthoai,
                                  ct.NgayNhan, ct.MaBao, ct.TenBao,
                                  ct.SoLuongThuc, ct.SoLuongDu,
                                  ct.ThanhTien, ct.DonGia
                           FROM TabKhachhang kh
                           INNER JOIN TabHoadon hd ON kh.MaKH = hd.MaKH
                           INNER JOIN TabChiTietHoaDon ct ON hd.SoHD = ct.SoHD
                           WHERE kh.MaKH = @maKH
                             AND CAST(ct.NgayNhan AS DATE) >= @tu
                             AND CAST(ct.NgayNhan AS DATE) <= @den
                           ORDER BY ct.NgayNhan DESC";

            var ps = new List<SqlParameter>
            {
                new SqlParameter("@topN", SqlDbType.Int) { Value = topN },
                new SqlParameter("@maKH", SqlDbType.VarChar) { Value = maKh },
                new SqlParameter("@tu", SqlDbType.Date) { Value = tuNgayDt.Date },
                new SqlParameter("@den", SqlDbType.Date) { Value = denNgayDt.Date }
            };

            var dt = await DbHelper.Instance.FillDataTableAsync(sql, ct, ps.ToArray());

            if (dt.Rows.Count == 0)
                return new ToolResult { Success = true, Output = $"Khách hàng [{maKh}] không có lịch giao báo từ {tuNgayDt:yyyy-MM-dd} đến {denNgayDt:yyyy-MM-dd}." };

            string tenKH = dt.Rows[0]["Ten"]?.ToString() ?? "";
            string dienthoai = dt.Rows[0]["Dienthoai"]?.ToString() ?? "";
            var lines = new List<string>
            {
                $"Lịch giao báo của [{maKh}] {tenKH} | ĐT: {dienthoai} | Từ {tuNgayDt:yyyy-MM-dd} đến {denNgayDt:yyyy-MM-dd} (hiển thị {dt.Rows.Count} dòng):"
            };
            int stt = 1;
            foreach (DataRow row in dt.Rows)
            {
                DateTime ngay = row["NgayNhan"] != DBNull.Value ? Convert.ToDateTime(row["NgayNhan"]) : DateTime.MinValue;
                string maBao = row["MaBao"]?.ToString() ?? "";
                string tenBao = row["TenBao"]?.ToString() ?? "";
                int slBan = row["SoLuongThuc"] != DBNull.Value ? Convert.ToInt32(row["SoLuongThuc"]) : 0;
                int slDu = row["SoLuongDu"] != DBNull.Value ? Convert.ToInt32(row["SoLuongDu"]) : 0;
                decimal tt = row["ThanhTien"] != DBNull.Value ? Convert.ToDecimal(row["ThanhTien"]) : 0;
                lines.Add($"#{stt++} {ngay:yyyy-MM-dd} | [{maBao}] {tenBao} | Bán: {slBan} | Dư: {slDu} | Tiền: {tt:N0}đ");
            }

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }

        private async Task<ToolResult> GetChurnRiskCustomersAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            parameters.TryGetValue("soNgayKhongMua", out var soNgayObj);
            parameters.TryGetValue("topN", out var topNObj);

            int soNgay = 90;
            if (soNgayObj != null && int.TryParse(soNgayObj.ToString(), out var d) && d > 0)
                soNgay = Math.Min(d, 365 * 5);

            int topN = 20;
            if (topNObj != null && int.TryParse(topNObj.ToString(), out var n) && n > 0)
                topN = Math.Min(n, 100);

            string sql = @"SELECT TOP (@topN) kh.MaKH, kh.Ten, kh.Dienthoai,
                                  MAX(hd.NgayLapPhieu) AS LanCuoiMua,
                                  DATEDIFF(DAY, MAX(hd.NgayLapPhieu), GETDATE()) AS SoNgayKhongMua,
                                  COUNT(DISTINCT hd.SoHD) AS TongSoHoaDon,
                                  ISNULL(SUM(ct.ThanhTien), 0) AS TongDoanhThu
                           FROM TabKhachhang kh
                           INNER JOIN TabHoadon hd ON kh.MaKH = hd.MaKH
                           INNER JOIN TabChiTietHoaDon ct ON hd.SoHD = ct.SoHD
                           WHERE hd.ThanhToan = 1
                           GROUP BY kh.MaKH, kh.Ten, kh.Dienthoai
                           HAVING DATEDIFF(DAY, MAX(hd.NgayLapPhieu), GETDATE()) >= @soNgay
                           ORDER BY SoNgayKhongMua DESC";

            var ps = new List<SqlParameter>
            {
                new SqlParameter("@topN", SqlDbType.Int) { Value = topN },
                new SqlParameter("@soNgay", SqlDbType.Int) { Value = soNgay }
            };

            var dt = await DbHelper.Instance.FillDataTableAsync(sql, ct, ps.ToArray());

            if (dt.Rows.Count == 0)
                return new ToolResult { Success = true, Output = $"Không có khách hàng nào ngừng mua ≥ {soNgay} ngày." };

            var lines = new List<string> { $"Top {dt.Rows.Count} khách hàng ngừng mua ≥ {soNgay} ngày (rủi ro bỏ):" };
            int stt = 1;
            foreach (DataRow row in dt.Rows)
            {
                string ma = row["MaKH"]?.ToString() ?? "";
                string ten = row["Ten"]?.ToString() ?? "";
                string dienthoai = row["Dienthoai"]?.ToString() ?? "";
                DateTime lanCuoi = row["LanCuoiMua"] != DBNull.Value ? Convert.ToDateTime(row["LanCuoiMua"]) : DateTime.MinValue;
                int soNgayKM = row["SoNgayKhongMua"] != DBNull.Value ? Convert.ToInt32(row["SoNgayKhongMua"]) : 0;
                int tongHD = row["TongSoHoaDon"] != DBNull.Value ? Convert.ToInt32(row["TongSoHoaDon"]) : 0;
                decimal tongDT = row["TongDoanhThu"] != DBNull.Value ? Convert.ToDecimal(row["TongDoanhThu"]) : 0;
                lines.Add($"#{stt++} [{ma}] {ten} | ĐT: {dienthoai} | Lần cuối: {lanCuoi:yyyy-MM-dd} | {soNgayKM} ngày | Tổng {tongHD} HĐ | DT lũy kế: {tongDT:N0}đ");
            }

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }

        private async Task<ToolResult> GetProfitMarginByPublicationAsync(Dictionary<string, object> parameters, CancellationToken ct)
        {
            parameters.TryGetValue("topN", out var topNObj);
            parameters.TryGetValue("tuNgay", out var tuNgayObj);
            parameters.TryGetValue("denNgay", out var denNgayObj);

            int topN = 10;
            if (topNObj != null && int.TryParse(topNObj.ToString(), out var n) && n > 0)
                topN = Math.Min(n, 50);

            string tuNgay = tuNgayObj?.ToString()?.Trim() ?? string.Empty;
            string denNgay = denNgayObj?.ToString()?.Trim() ?? string.Empty;

            string sql = @"SELECT TOP (@topN) b.MaBao, b.Ten,
                                  ISNULL(SUM(ct.ThanhTien), 0) AS DoanhThu,
                                  ISNULL(SUM(ct.SoLuongThuc * b.DonGia * 0.85), 0) AS ChiPhiUocTinh,
                                  ISNULL(SUM(ct.ThanhTien) - SUM(ct.SoLuongThuc * b.DonGia * 0.85), 0) AS LoiNhuan,
                                  CAST(ISNULL((SUM(ct.ThanhTien) - SUM(ct.SoLuongThuc * b.DonGia * 0.85)) * 100.0 / NULLIF(SUM(ct.ThanhTien), 0), 0) AS DECIMAL(5,2)) AS TySuatLN
                           FROM TabBao b
                           INNER JOIN TabChiTietHoaDon ct ON b.MaBao = ct.MaBao
                           INNER JOIN TabHoadon hd ON ct.SoHD = hd.SoHD
                           WHERE hd.ThanhToan = 1";

            var ps = new List<SqlParameter>
            {
                new SqlParameter("@topN", SqlDbType.Int) { Value = topN }
            };

            DateTime tuNgayDt, denNgayDt;
            bool hasTu = DateTime.TryParse(tuNgay, out tuNgayDt);
            bool hasDen = DateTime.TryParse(denNgay, out denNgayDt);

            if (hasTu)
            {
                sql += " AND CAST(hd.NgayLapPhieu AS DATE) >= @tu";
                ps.Add(new SqlParameter("@tu", SqlDbType.Date) { Value = tuNgayDt.Date });
            }
            if (hasDen)
            {
                sql += " AND CAST(hd.NgayLapPhieu AS DATE) <= @den";
                ps.Add(new SqlParameter("@den", SqlDbType.Date) { Value = denNgayDt.Date });
            }

            sql += @" GROUP BY b.MaBao, b.Ten
                      ORDER BY LoiNhuan DESC";

            var dt = await DbHelper.Instance.FillDataTableAsync(sql, ct, ps.ToArray());

            if (dt.Rows.Count == 0)
                return new ToolResult { Success = true, Output = "Không có dữ liệu lợi nhuận theo đầu báo." };

            var lines = new List<string> { $"Top {dt.Rows.Count} đầu báo theo lợi nhuận gộp ước tính (chi phí ≈ 85% doanh thu):" };
            decimal tongLN = 0;
            int stt = 1;
            foreach (DataRow row in dt.Rows)
            {
                string ma = row["MaBao"]?.ToString() ?? "";
                string ten = row["Ten"]?.ToString() ?? "";
                decimal dtVal = row["DoanhThu"] != DBNull.Value ? Convert.ToDecimal(row["DoanhThu"]) : 0;
                decimal cp = row["ChiPhiUocTinh"] != DBNull.Value ? Convert.ToDecimal(row["ChiPhiUocTinh"]) : 0;
                decimal ln = row["LoiNhuan"] != DBNull.Value ? Convert.ToDecimal(row["LoiNhuan"]) : 0;
                decimal tyLe = row["TySuatLN"] != DBNull.Value ? Convert.ToDecimal(row["TySuatLN"]) : 0;
                tongLN += ln;
                lines.Add($"#{stt++} [{ma}] {ten} | DT: {dtVal:N0}đ | CP: {cp:N0}đ | LN: {ln:N0}đ | Tỷ suất: {tyLe:N2}%");
            }
            lines.Add($"--- Tổng LN: {tongLN:N0}đ ---");

            return new ToolResult { Success = true, Output = string.Join("\n", lines) };
        }
    }
}

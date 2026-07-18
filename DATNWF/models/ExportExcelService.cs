using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace DATNWF.Models
{
    public class ExportExcelService
    {
        private readonly DbHelper _db = DbHelper.Instance;

        public Task<string> ExportHoaDonAsync(string soHD, string customFolder = null)
        {
            return Task.Run(() =>
            {
                try
                {
                    var hdRow = GetHoaDonInfo(soHD);
                    if (hdRow == null) throw new InvalidOperationException($"Không tìm thấy hóa đơn: {soHD}");

                string tenKH = hdRow["tenKhachHang"]?.ToString() ?? "";
                string sdt = hdRow["sdtKhachHang"]?.ToString() ?? "";
                decimal ck = hdRow["chietKhau"] != DBNull.Value ? Convert.ToDecimal(hdRow["chietKhau"]) : 0;
                var dtTuNgay = hdRow["tuNgay"] as DateTime? ?? DateTime.MinValue;
                var dtDenNgay = hdRow["denNgay"] as DateTime? ?? DateTime.MinValue;

                DataTable dtChiTiet = _db.FillDataTable(
                    @"SELECT sohd, ngayNhan, maBao, tenBao, soBao,
                             soLuongThuc, soLuongDu, donGia, thanhTien, dieuPhoi
                      FROM dbo.tabCHITIETHOADON
                      WHERE sohd = @soHD
                      ORDER BY ngayNhan ASC",
                    new System.Data.SqlClient.SqlParameter("@soHD", soHD));

                using var wb = new XLWorkbook();

                // ── Sheet 1: Chi tiết hóa đơn ──────────────────────────
                var ws1 = wb.Worksheets.Add("Chi tiết hóa đơn");
                int row = 1;

                // Header công ty
                ws1.Cell(row, 1).Value = "HÓA ĐƠN BÁO";
                ws1.Cell(row, 1).Style.Font.FontSize = 16;
                ws1.Cell(row, 1).Style.Font.Bold = true;
                ws1.Range(row, 1, row, 8).Merge();
                row++;

                ws1.Cell(row, 1).Value = $"Số HĐ: {soHD}";
                ws1.Cell(row, 1).Style.Font.Bold = true;
                ws1.Range(row, 1, row, 8).Merge();
                row++;

                // Thông tin khách hàng
                ws1.Cell(row, 1).Value = "Khách hàng:";
                ws1.Cell(row, 1).Style.Font.Bold = true;
                ws1.Cell(row, 2).Value = tenKH;
                ws1.Range(row, 2, row, 7).Merge();
                row++;

                ws1.Cell(row, 1).Value = "Điện thoại:";
                ws1.Cell(row, 1).Style.Font.Bold = true;
                ws1.Cell(row, 2).Value = sdt;
                row++;

                ws1.Cell(row, 1).Value = "Kỳ:";
                ws1.Cell(row, 1).Style.Font.Bold = true;
                ws1.Cell(row, 2).Value = $"{dtTuNgay:dd/MM/yyyy} – {dtDenNgay:dd/MM/yyyy}";
                ws1.Range(row, 2, row, 7).Merge();
                row++;

                ws1.Cell(row, 1).Value = "Chiết khấu:";
                ws1.Cell(row, 1).Style.Font.Bold = true;
                ws1.Cell(row, 2).Value = ck > 0 ? $"{ck:N0}%" : "Không";
                ws1.Range(row, 2, row, 7).Merge();
                row++;

                row++;

                // Header bảng gộp
                string[] headers = { "Mã báo", "Tên báo", "Ngày nhận", "Tổng SL", "Điều phối", "Dư", "Đơn giá", "Thành tiền" };
                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = ws1.Cell(row, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#F0A500");
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Font.FontColor = XLColor.White;
                }
                row++;

                // Gộp theo báo
                var grouped = dtChiTiet.AsEnumerable()
                    .GroupBy(r => new { maBao = r["maBao"]?.ToString(), tenBao = r["tenBao"]?.ToString() })
                    .Select(g =>
                    {
                        var rows = g.OrderBy(r => r["ngayNhan"]).ToList();
                        DateTime? ngayDau = rows.First()["ngayNhan"] != DBNull.Value
                            ? (DateTime)rows.First()["ngayNhan"] : (DateTime?)null;
                        DateTime? ngayCuoi = rows.Last()["ngayNhan"] != DBNull.Value
                            ? (DateTime)rows.Last()["ngayNhan"] : (DateTime?)null;
                        return new
                        {
                            g.Key.maBao,
                            g.Key.tenBao,
                            NgayDau = ngayDau,
                            NgayCuoi = ngayCuoi,
                            TongSL = g.Sum(r => Convert.ToInt32(r["soLuongThuc"])),
                            TongDieuPhoi = g.Sum(r => Convert.ToInt32(r["dieuPhoi"])),
                            TongDu = g.Sum(r => Convert.ToInt32(r["soLuongDu"])),
                            DonGia = g.First()["donGia"] != DBNull.Value ? Convert.ToDecimal(g.First()["donGia"]) : 0,
                            ThanhTien = g.Sum(r => Convert.ToDecimal(r["thanhTien"]))
                        };
                    })
                    .OrderBy(x => x.NgayDau)
                    .ToList();

                decimal tongTruocCK = 0;
                foreach (var g in grouped)
                {
                    string ngayText = g.NgayDau.HasValue && g.NgayCuoi.HasValue && g.NgayDau != g.NgayCuoi
                        ? $"{g.NgayDau:dd/MM} – {g.NgayCuoi:dd/MM/yyyy}"
                        : g.NgayDau.HasValue ? $"{g.NgayDau:dd/MM/yyyy}" : "";

                    ws1.Cell(row, 1).Value = g.maBao;
                    ws1.Cell(row, 2).Value = g.tenBao;
                    ws1.Cell(row, 3).Value = ngayText;
                    ws1.Cell(row, 4).Value = g.TongSL;
                    ws1.Cell(row, 5).Value = g.TongDieuPhoi;
                    ws1.Cell(row, 6).Value = g.TongDu;
                    ws1.Cell(row, 7).Value = g.DonGia;
                    ws1.Cell(row, 7).Style.NumberFormat.Format = "#,##0";
                    ws1.Cell(row, 8).Value = g.ThanhTien;
                    ws1.Cell(row, 8).Style.NumberFormat.Format = "#,##0";

                    tongTruocCK += g.ThanhTien;
                    row++;
                }

                // Tổng cộng
                row++;
                ws1.Cell(row, 7).Value = "Tổng trước CK:";
                ws1.Cell(row, 7).Style.Font.Bold = true;
                ws1.Cell(row, 8).Value = tongTruocCK;
                ws1.Cell(row, 8).Style.NumberFormat.Format = "#,##0";
                ws1.Cell(row, 8).Style.Font.Bold = true;
                row++;

                if (ck > 0)
                {
                    decimal tienCK = tongTruocCK * ck / 100;
                    decimal tongSauCK = tongTruocCK - tienCK;

                    ws1.Cell(row, 7).Value = $"Chiết khấu ({ck:N0}%):";
                    ws1.Cell(row, 7).Style.Font.Bold = true;
                    ws1.Cell(row, 8).Value = -tienCK;
                    ws1.Cell(row, 8).Style.NumberFormat.Format = "#,##0";
                    ws1.Cell(row, 8).Style.Font.Bold = true;
                    row++;

                    ws1.Cell(row, 7).Value = "TỔNG CỘNG:";
                    ws1.Cell(row, 7).Style.Font.Bold = true;
                    ws1.Cell(row, 7).Style.Font.FontSize = 12;
                    ws1.Cell(row, 8).Value = tongSauCK;
                    ws1.Cell(row, 8).Style.NumberFormat.Format = "#,##0";
                    ws1.Cell(row, 8).Style.Font.Bold = true;
                    ws1.Cell(row, 8).Style.Font.FontSize = 12;
                }
                else
                {
                    ws1.Cell(row, 7).Value = "TỔNG CỘNG:";
                    ws1.Cell(row, 7).Style.Font.Bold = true;
                    ws1.Cell(row, 7).Style.Font.FontSize = 12;
                    ws1.Cell(row, 8).Value = tongTruocCK;
                    ws1.Cell(row, 8).Style.NumberFormat.Format = "#,##0";
                    ws1.Cell(row, 8).Style.Font.Bold = true;
                    ws1.Cell(row, 8).Style.Font.FontSize = 12;
                }

                // Auto fit columns
                ws1.Columns().AdjustToContents();

                // ── Sheet 2: Danh sách hóa đơn ─────────────────────────
                DataTable dtAllHD = _db.FillDataTable(Queries.HoaDonList);

                var ws2 = wb.Worksheets.Add("Danh sách hóa đơn");
                int row2 = 1;

                ws2.Cell(row2, 1).Value = "DANH SÁCH HÓA ĐƠN";
                ws2.Cell(row2, 1).Style.Font.FontSize = 14;
                ws2.Cell(row2, 1).Style.Font.Bold = true;
                ws2.Range(row2, 1, row2, 7).Merge();
                row2++;

                string[] hdHeaders = { "Số HĐ", "Mã KH", "Tên khách hàng", "Ngày lập", "Từ ngày", "Đến ngày", "Thanh toán" };
                for (int i = 0; i < hdHeaders.Length; i++)
                {
                    var cell = ws2.Cell(row2, i + 1);
                    cell.Value = hdHeaders[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#F0A500");
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Font.FontColor = XLColor.White;
                }
                row2++;

                foreach (DataRow dr in dtAllHD.Rows)
                {
                    ws2.Cell(row2, 1).Value = dr["sohd"]?.ToString();
                    ws2.Cell(row2, 2).Value = dr["makh"]?.ToString();
                    ws2.Cell(row2, 3).Value = dr["tenKhachHang"]?.ToString();
                    ws2.Cell(row2, 4).Value = dr["ngayLapPhieu"] != DBNull.Value
                        ? ((DateTime)dr["ngayLapPhieu"]).ToString("dd/MM/yyyy") : "";
                    ws2.Cell(row2, 5).Value = dr["tuNgay"] != DBNull.Value
                        ? ((DateTime)dr["tuNgay"]).ToString("dd/MM/yyyy") : "";
                    ws2.Cell(row2, 6).Value = dr["denNgay"] != DBNull.Value
                        ? ((DateTime)dr["denNgay"]).ToString("dd/MM/yyyy") : "";
                    ws2.Cell(row2, 7).Value = dr["thanhToan"]?.ToString();

                    for (int i = 1; i <= 7; i++)
                        ws2.Cell(row2, i).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    row2++;
                }

                ws2.Columns().AdjustToContents();

                // Save
                string folder = string.IsNullOrEmpty(customFolder)
                    ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    : customFolder;
                string filePath = Path.Combine(folder, $"HoaDon_{soHD}_{Guid.NewGuid():N}.xlsx");
                wb.SaveAs(filePath);
                return filePath;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    throw;
                }
            });
        }

        private DataRow GetHoaDonInfo(string soHD)
        {
            DataTable dt = _db.FillDataTable(
                @"SELECT hd.sohd, hd.makh, hd.ngayLapPhieu, hd.tuNgay, hd.denNgay,
                         hd.ghichu, hd.thanhToan,
                         kh.TEN      AS tenKhachHang,
                         kh.DIENTHOAI AS sdtKhachHang,
                         kh.CHIETKHAU AS chietKhau
                  FROM dbo.tabHOADON hd
                  INNER JOIN dbo.tabKHACHHANG kh ON kh.MAKH = hd.makh
                  WHERE hd.sohd = @soHD",
                new System.Data.SqlClient.SqlParameter("@soHD", soHD));

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        private static string GetGhiChu(int slThuc, int slDư)
        {
            if (slDư < 0) return "⚠ THỪA";
            if (slDư > 0) return "⚠ THIẾU";
            return "✓ ĐỦ";
        }

        private static class Queries
        {
            public const string HoaDonList = @"
                SELECT TOP 50
                    hd.sohd, hd.makh, hd.ngayLapPhieu, hd.tuNgay, hd.denNgay,
                    hd.ghichu, hd.thanhToan,
                    kh.TEN       AS tenKhachHang,
                    kh.DIENTHOAI  AS sdtKhachHang,
                    kh.CHIETKHAU  AS chietKhau
                FROM dbo.tabHOADON hd
                INNER JOIN dbo.tabKHACHHANG kh ON kh.MAKH = hd.makh
                ORDER BY hd.ngayLapPhieu DESC, hd.sohd DESC";
        }
    }
}

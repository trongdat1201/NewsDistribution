using System;
using System.Data;
using System.Data.SqlClient;

namespace DATNWF.Models
{
    public class HoaDonService
    {
        private readonly DbHelper _db = DbHelper.Instance;

        public bool TonTaiHoaDon(string soHD)
        {
            return !string.IsNullOrEmpty(_db.ExecuteScalar<string>(
                "SELECT sohd FROM dbo.tabHOADON WHERE sohd = @soHD",
                new SqlParameter("@soHD", soHD)));
        }

        public DataRow LayThongTinDieuPhoi(string soHD)
        {
            const string query = @"SELECT dp.soHD, dp.makh, dp.ngay, dp.tungay, dp.denngay, dp.ghiChu,
                                         kh.ten AS tenKhachHang
                                  FROM dbo.tabDieuPhoi dp
                                  INNER JOIN dbo.tabKHACHHANG kh ON kh.makh = dp.makh
                                  WHERE dp.soHD = @soHD";

            DataTable dt = _db.FillDataTable(query, new SqlParameter("@soHD", soHD));
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public DataTable LayChiTietDieuPhoi(string soHD)
        {
            const string query = @"SELECT sohd, ngayNhan, maBao, tenbao, sobao,
                                         soluongDieuPhoi, soluongBan, donGia
                                  FROM dbo.tabChiTietDieuPhoi
                                  WHERE sohd = @soHD";

            return _db.FillDataTable(query, new SqlParameter("@soHD", soHD));
        }

        public (int SoLuongThuc, int SoLuongDu, int DieuPhoi, decimal DonGia, decimal ThanhTien)
            TinhChiTiet(int slBan, int slDieuPhoi, decimal donGia)
        {
            // soLuongThuc = MAX(soluongDieuPhoi, soluongBan) — lấy theo cái lớn hơn
            int soLuongThuc    = Math.Max(slBan, slDieuPhoi);
            // soLuongDu = phần dư khi điều phối nhiều hơn bán (luôn ≥ 0)
            int soLuongDu      = slDieuPhoi > slBan ? slDieuPhoi - slBan : 0;
            // dieuPhoi = phần phát sinh khi bán vượt chỉ tiêu (bán nhiều hơn cấp ban đầu)
            int dieuPhoi       = slBan > slDieuPhoi ? slBan - slDieuPhoi : 0;
            decimal thanhTien  = soLuongThuc * donGia;
            return (soLuongThuc, soLuongDu, dieuPhoi, donGia, thanhTien);
        }
    }
}

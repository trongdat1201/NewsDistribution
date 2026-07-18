using System.Data;
using DATNWF.Models;

namespace DATNWF.Models.AI
{
    /// <summary>
    /// Cung cấp business schema mô tả các bảng DB DATNWF cho AI system prompt.
    /// Single source of truth - tránh schema bị hard-code lặp ở nhiều file.
    /// </summary>
    public static class BusinessContextProvider
    {
        public static string GetSchemaDescription()
        {
            return
@"DATNWF là hệ thống quản lý phân phối báo/tạp chí.
Các bảng chính:
- TabKhachhang(Makh, Ten, Diachi, Dienthoai, Chietkhau, P_PH, P_KT, Uutien) — thông tin khách hàng
- TabBao(MaBao, Ten, Dvt, DonGia, NgayBatDau, Thu1..Thu7, SoLanPhtrongTuan, Sogoc) — danh mục báo
- TabHoadon(Sohd, Makh, NgayLapPhieu, TuNgay, DenNgay, ThanhToan) — hóa đơn (ThanhToan=1 là đã trả)
- TabTon(Ngay, MaBao, SoBao, SlPhatHanh, Banthuc, BanLe, DieuPhoi, Ton) — tồn kho theo ngày
- TabChitiethoadon(Sohd, MaBao, TenBao, SoBao, SoLuongThuc, SoLuongDu, DonGia, ThanhTien, DieuPhoi) — chi tiết hóa đơn (SL thực = bán được, SL dư = trả lại)
- TabDieuPhoi(SoHD, Makh, Ngay, TuNgay, DenNgay, GhiChu) — phiếu điều phối giao báo
- TabChiTietDieuPhoi(SoHD, NgayNhan, MaBao, TenBao, SoBao, SoluongDieuPhoi, SoluongBan, ThanhTien) — chi tiết điều phối";
        }

        /// <summary>
        /// Đọc schema thật từ DB (cache 5 phút) — dùng khi cần update realtime.
        /// </summary>
        public static DataTable GetLiveSchema()
        {
            string sql = @"
                SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME IN ('TabKhachhang', 'TabBao', 'TabHoadon', 'TabTon',
                                     'TabChitiethoadon', 'TabDieuPhoi', 'TabChiTietDieuPhoi')
                ORDER BY TABLE_NAME, ORDINAL_POSITION";
            return DbHelper.Instance.FillDataTable(sql);
        }
    }
}

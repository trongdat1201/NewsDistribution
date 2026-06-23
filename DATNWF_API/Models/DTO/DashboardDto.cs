using System.Collections.Generic;

namespace DATNWF_API.Models.DTO // CHÚ Ý: Phải là namespace của project API
{
    public class DashboardSummaryDto
    {
        public decimal TongDoanhThu { get; set; }
        public int TongSoBao { get; set; }
        public int TongKhachHang { get; set; }

        public List<ThongKeTronDto> TyTrongDoanhThu { get; set; } = new List<ThongKeTronDto>();
        public List<ThongKeDuongDto> BienDongDoanhThu { get; set; } = new List<ThongKeDuongDto>();
        public List<ThongKeCotDto> ThongKeTonKho { get; set; } = new List<ThongKeCotDto>();
        public List<TopKhachHangDto> KhachHangTiemNang { get; set; } = new List<TopKhachHangDto>();
    }

    public class ThongKeTronDto { public string TenBao { get; set; } = string.Empty; public decimal DoanhThu { get; set; } }
    public class ThongKeDuongDto { public string Ngay { get; set; } = string.Empty; public double DoanhThu { get; set; } }
    public class ThongKeCotDto { public string Ngay { get; set; } = string.Empty; public double PhatHanh { get; set; } public double TieuThu { get; set; } public double TonKho { get; set; } }
    public class TopKhachHangDto { public string TenKhachHang { get; set; } = string.Empty; public double SoLuongMua { get; set; } }
}
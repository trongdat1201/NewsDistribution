using System;

namespace DATNWF.Models.DTO // Nhớ đổi thành DATNWF_API.Models.DTO khi dán vào project API nhé
{
    public class TonDto
    {
        public DateTime? Ngay { get; set; }
        public string MaBao { get; set; } = string.Empty;
        public string TenBao { get; set; } = string.Empty;
        public int? SoBao { get; set; }
        public int? SlPhatHanh { get; set; }
        public int? Banthuc { get; set; }
        public int? BanLe { get; set; }
        public int? DieuPhoi { get; set; }
        public int? Ton { get; set; }
    }
}
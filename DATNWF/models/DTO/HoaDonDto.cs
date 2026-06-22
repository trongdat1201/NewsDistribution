using System;

namespace DATNWF.Models.DTO
{
    public class HoaDonDto
    {
        public string Sohd { get; set; } = string.Empty;
        public string Makh { get; set; } = string.Empty;
        public DateTime NgayLapPhieu { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public string Ghichu { get; set; } = string.Empty;
        public bool ThanhToan { get; set; }
    }
}

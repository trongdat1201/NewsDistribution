using System;

namespace DATNWF.Models.DTO
{
    public class DieuPhoiDto
    {
        public string Sohd { get; set; }
        public string Makh { get; set; }
        public DateTime? NgayLapPhieu { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public string GhiChu { get; set; }
    }
}
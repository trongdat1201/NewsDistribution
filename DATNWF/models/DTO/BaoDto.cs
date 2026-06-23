using System;

namespace DATNWF.Models.DTO
{
    public class BaoDto
    {
        public string MaBao { get; set; } = string.Empty;
        public string Ten { get; set; } = string.Empty;
        public string Dvt { get; set; } = string.Empty;
        public double DonGia { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public bool? Thu1 { get; set; }
        public bool? Thu2 { get; set; }
        public bool? Thu3 { get; set; }
        public bool? Thu4 { get; set; }
        public bool? Thu5 { get; set; }
        public bool? Thu6 { get; set; }
        public bool? Thu7 { get; set; }
        public int? SoLanPhtrongTuan { get; set; }
        public int? Sogoc { get; set; }
    }
}

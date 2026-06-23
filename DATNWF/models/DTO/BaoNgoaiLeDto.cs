using System;

namespace DATNWF.Models.DTO
{
    public class BaoNgoaiLeDto
    {
        public string MaBao { get; set; } = string.Empty;
        public DateTime NgayPhatHanh { get; set; }
        public int? SoLanTrongNam { get; set; }
    }
}

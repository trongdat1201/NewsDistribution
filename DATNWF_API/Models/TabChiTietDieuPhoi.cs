using System;
using System.Collections.Generic;

namespace DATNWF_API.Models;

public partial class TabChiTietDieuPhoi
{
    public string Sohd { get; set; } = null!;

    public DateTime NgayNhan { get; set; }

    public string MaBao { get; set; } = null!;

    public string? Tenbao { get; set; }

    public int? Sobao { get; set; }

    public decimal? DonGia { get; set; }

    public int? SoluongDieuPhoi { get; set; }

    public int? SoluongBan { get; set; }

    public decimal? ThanhTien { get; set; }

    public virtual TabBao MaBaoNavigation { get; set; } = null!;

    public virtual TabDieuPhoi SohdNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace DATNWF_API.Models;

public partial class TabChitiethoadon
{
    public string Sohd { get; set; } = null!;

    public DateTime NgayNhan { get; set; }

    public string MaBao { get; set; } = null!;

    public string? TenBao { get; set; }

    public int? SoBao { get; set; }

    public int? SoLuongThuc { get; set; }

    public int? SoLuongDu { get; set; }

    public double? DonGia { get; set; }

    public double? ThanhTien { get; set; }

    public int? DieuPhoi { get; set; }

    public int? Soluongphatsinh1 { get; set; }

    public virtual TabBao MaBaoNavigation { get; set; } = null!;

    public virtual TabHoadon SohdNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace DATNWF_API.Models;

public partial class TabTon
{
    public DateTime Ngay { get; set; }

    public string MaBao { get; set; } = null!;

    public string? TenBao { get; set; }

    public int? SoBao { get; set; }

    public int? SlPhatHanh { get; set; }

    public int? Banthuc { get; set; }

    public int? BanLe { get; set; }

    public int? DieuPhoi { get; set; }

    public int? Ton { get; set; }

    public virtual TabBao MaBaoNavigation { get; set; } = null!;
}

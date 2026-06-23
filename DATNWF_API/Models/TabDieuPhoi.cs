using System;
using System.Collections.Generic;

namespace DATNWF_API.Models;

public partial class TabDieuPhoi
{
    public string SoHd { get; set; } = null!;

    public string? Makh { get; set; }

    public DateTime? Ngay { get; set; }

    public DateTime? Tungay { get; set; }

    public DateTime? Denngay { get; set; }

    public string? GhiChu { get; set; }

    public virtual TabKhachhang? MakhNavigation { get; set; }

    public virtual ICollection<TabChiTietDieuPhoi> TabChiTietDieuPhois { get; set; } = new List<TabChiTietDieuPhoi>();
}

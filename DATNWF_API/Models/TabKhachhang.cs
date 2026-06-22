using System;
using System.Collections.Generic;

namespace DATNWF_API.Models;

public partial class TabKhachhang
{
    public string Makh { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public string? Diachi { get; set; }

    public string? Dienthoai { get; set; }

    public short Chietkhau { get; set; }

    public bool PPh { get; set; }

    public bool PKt { get; set; }

    public string? Uutien { get; set; }

    public virtual ICollection<TabDieuPhoi> TabDieuPhois { get; set; } = new List<TabDieuPhoi>();

    public virtual ICollection<TabHoadon> TabHoadons { get; set; } = new List<TabHoadon>();
}

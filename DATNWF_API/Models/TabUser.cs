using System;
using System.Collections.Generic;

namespace DATNWF_API.Models;

public partial class TabUser
{
    public string TenDangNhap { get; set; } = null!;

    public string? HoTen { get; set; }

    public string? MatKhau { get; set; }

    public bool? Ht1 { get; set; }

    public bool? St1 { get; set; }

    public bool? St2 { get; set; }

    public bool? Nv1 { get; set; }

    public bool? Nv2 { get; set; }

    public bool? Nv3 { get; set; }

    public bool? Nv4 { get; set; }

    public bool? Nv5 { get; set; }

    public bool? Bc1 { get; set; }

    public bool? Bc2 { get; set; }

    public bool? Bc3 { get; set; }
}

using System;
using System.Collections.Generic;

namespace DATNWF_API.Models;

public partial class TabLogin
{
    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public bool Ht { get; set; }

    public bool Nv { get; set; }

    public bool Bc { get; set; }
}

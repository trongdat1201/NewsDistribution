using System;
using System.Collections.Generic;

namespace DATNWF_API.Models;

public partial class TabKhachhang1
{
    public string Makh { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string? DienThoai { get; set; }

    public short ChietKhau { get; set; }

    public bool PPh { get; set; }

    public bool PKt { get; set; }

    public string? UuTien { get; set; }
}

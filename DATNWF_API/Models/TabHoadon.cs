using System;
using System.Collections.Generic;

namespace DATNWF_API.Models;

public partial class TabHoadon
{
    public string Sohd { get; set; } = null!;

    public string Makh { get; set; } = null!;

    public DateTime NgayLapPhieu { get; set; }

    public DateTime TuNgay { get; set; }

    public DateTime DenNgay { get; set; }

    public string? Ghichu { get; set; }

    public bool ThanhToan { get; set; }

    public virtual TabKhachhang MakhNavigation { get; set; } = null!;
}

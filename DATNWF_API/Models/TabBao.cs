using System;
using System.Collections.Generic;

namespace DATNWF_API.Models;

public partial class TabBao
{
    public string MaBao { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public string Dvt { get; set; } = null!;

    public double DonGia { get; set; }

    public DateTime? NgayBatDau { get; set; }

    public bool? Thu1 { get; set; }

    public bool? Thu2 { get; set; }

    public bool? Thu3 { get; set; }

    public bool? Thu4 { get; set; }

    public bool? Thu5 { get; set; }

    public bool? Thu6 { get; set; }

    public bool? Thu7 { get; set; }

    public int? SoLanPhtrongTuan { get; set; }

    public int? Sogoc { get; set; }

    public virtual ICollection<TabBaoNgoaiLe> TabBaoNgoaiLes { get; set; } = new List<TabBaoNgoaiLe>();

    public virtual ICollection<TabChiTietDieuPhoi> TabChiTietDieuPhois { get; set; } = new List<TabChiTietDieuPhoi>();

    public virtual ICollection<TabTon> TabTons { get; set; } = new List<TabTon>();
}

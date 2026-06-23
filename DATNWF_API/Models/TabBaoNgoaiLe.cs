using System;
using System.Collections.Generic;

namespace DATNWF_API.Models;

public partial class TabBaoNgoaiLe
{
    public string MaBao { get; set; } = null!;

    public DateTime NgayPhatHanh { get; set; }

    public int? SoLanTrongNam { get; set; }

    public virtual TabBao MaBaoNavigation { get; set; } = null!;
}

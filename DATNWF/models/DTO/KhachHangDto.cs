using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DATNWF.Models.DTO
{
    // Class hứng dữ liệu danh sách khách hàng
    public class KhachHangDto
    {
        public string MaKH { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public short ChietKhau { get; set; }

        [JsonProperty("PPh")]
        public bool P_PH { get; set; }

        [JsonProperty("PKt")]
        public bool P_KT { get; set; }

        public string Uutien { get; set; }
    }

    // Class hứng dữ liệu khách order gần đây
    public class KhachHangGanDayDto
    {
        public string MaKH { get; set; }
        public string Ten { get; set; }
    }
}

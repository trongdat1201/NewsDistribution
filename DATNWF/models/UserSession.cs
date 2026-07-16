using System;

namespace DATNWF.Models
{
    public static class UserSession
    {
        public static string Username { get; set; }
        public static string Role { get; set; }

        // CÁC THUỘC TÍNH TƯƠNG THÍCH NGƯỢC CHO GIAO DIỆN CŨ
        public static bool IsHT => Role == "ROLE_HT";
        public static bool IsNV => Role == "ROLE_NV_PH";
        public static bool IsBC => Role == "ROLE_NV_KT";

        public static string JwtToken { get; set; }

        public static void Clear()
        {
            Username = null;
            Role = null;
            JwtToken = null;
            ApiClient.Instance.SetToken(null);
        }
    }
}

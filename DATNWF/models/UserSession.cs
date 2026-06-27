using System;

namespace DATNWF.Models
{
    public static class UserSession
    {
        public static string Username { get; set; }
        public static bool IsHT { get; set; }
        public static bool IsNV { get; set; }
        public static bool IsBC { get; set; }

        public static void Clear()
        {
            Username = null;
            IsHT = false;
            IsNV = false;
            IsBC = false;
        }
    }
}

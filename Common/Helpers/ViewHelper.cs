using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.Helpers
{
    public class ViewHelper
    {
        public static string TrangThai(int tt)
        {
            string tem = " làm dữ liệu";
            if (tt == 0) return "Chưa";
            if (tt == 1) return "Đang";
            if (tt == 2) return "Đã ";
            return "file được bảo vệ";
        }
    }
}
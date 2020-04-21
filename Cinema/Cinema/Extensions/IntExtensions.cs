using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cinema.Extensions
{
    public static class IntExtensions
    {
        public static string ToDuration(this int? value)
        {
            if (value == null) return string.Empty;
            var hours = Math.Truncate(value.Value / 60f);
            var minutes = value % 60;
            return $"{hours}h {minutes}m";
        }
    }
}
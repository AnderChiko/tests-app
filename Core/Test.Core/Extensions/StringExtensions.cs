using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static string ToTitleCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return char.ToUpperInvariant(value[0]) + value.Substring(1);
        }

        public static int ToInt(this string value)
        {
            int result = 0;
            // if conversion fails returns default
            if (string.IsNullOrEmpty(value))
                return result;

            try
            {
                result = int.Parse(value);
            }
            catch
            {
                result = 0;
            }
            return Convert.ToInt32(value);
        }

        public static int? ToNullableInt(this string value)
        {
            int? result = null;
            // if conversion fails returns default
            if (string.IsNullOrEmpty(value))
                return result;

            try
            {
                result = int.Parse(value);
            }
            catch
            {
                result = null;
            }
            return Convert.ToInt32(value);
        }
    }
}
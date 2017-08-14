using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneTestApi.Utils
{
    public static class Extensions
    {
        public static T ToEnum<T>(this string value, bool ignoreCase = true)
        {
            return (T) Enum.Parse(typeof(T), value, ignoreCase);
        }
    }
}
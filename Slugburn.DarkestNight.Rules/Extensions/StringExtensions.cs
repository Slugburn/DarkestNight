using System;
using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Extensions
{
    public static class StringExtensions
    {
        public static string ToCsv(this IEnumerable<string> values)
        {
            return string.Join(", ", values);
        }

        public static T ToEnum<T>(this string value)
        {
            return (T) Enum.Parse(typeof (T), value);
        }
    }
}

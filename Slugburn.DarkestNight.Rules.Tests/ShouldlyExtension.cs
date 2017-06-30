using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public static class ShouldlyExtension
    {
        public static void ShouldBeEquivalent<T>(this IEnumerable<T> actual, IEnumerable<T> expected) where T:IComparable<T>
        {
            var one = actual.OrderBy(x => x);
            var two = expected.OrderBy(x => x);
            one.ShouldBe(two);
        }

        public static void ShouldBeIfNotNull<T>(this T actual, T? expected, string message) where T:struct
        {
            if (expected.HasValue)
                actual.ShouldBe(expected.Value, message);
        }
    }
}

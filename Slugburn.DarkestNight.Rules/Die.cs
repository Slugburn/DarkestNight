using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules
{
    public static class Die
    {
        static readonly Random Random = new Random();

        public static int Roll()
        {
            return Random.Next(1, 6);
        }

        public static IEnumerable<int> Roll(int number)
        {
            return Enumerable.Range(1, number).Select(x => Roll());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.Rolls
{
    public static class Die
    {
        public static int Roll()
        {
            return Implementation.Roll();
        }

        public static List<int> Roll(int number)
        {
            return Enumerable.Range(1, number).Select(x=>Implementation.Roll()).ToList();
        }

        private class RandomDie : IDie
        {
            private static readonly Random Random = new Random();

            int IDie.Roll()
            {
                return Random.Next(1, 6);
            }
        }

        public static IDie Implementation { get; set; } = new RandomDie();
    }
}

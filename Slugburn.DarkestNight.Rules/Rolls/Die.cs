using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.Rolls
{
    public static class Die
    {
        private static IDie _implementation = new RandomDie();

        public static int Roll()
        {
            return _implementation.Roll();
        }

        public static List<int> Roll(int number)
        {
            return Enumerable.Range(1, number).Select(x=>_implementation.Roll()).ToList();
        }

        public static void SetImplementation(IDie die)
        {
            _implementation = die;
        }

        private class RandomDie : IDie
        {
            private static readonly Random Random = new Random();

            int IDie.Roll()
            {
                return Random.Next(1, 6);
            }
        }
    }
}

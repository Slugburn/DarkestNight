using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules
{
    public static class DiceExtension
    {
        public static ICollection<int> AddOneToHighest(this ICollection<int> roll)
        {
            var originalRoll = roll.OrderByDescending(x => x).ToList();
            var newRoll = new[] {originalRoll.First() + 1}.Concat(originalRoll.Skip(1)).ToList();
            return newRoll;
        }

        public static ICollection<int> AddOneToEach(this ICollection<int> roll)
        {
            return roll.Select(x => x + 1).ToList();
        }
    }
}

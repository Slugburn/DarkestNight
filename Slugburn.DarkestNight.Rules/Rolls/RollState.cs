using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.Rolls
{
    public class RollState
    {
        public static RollState Create(IEnumerable<int> roll)
        {
            var actualRoll = roll.ToList();
            var adjustedRoll = actualRoll.ToList();
            return new RollState {ActualRoll = actualRoll, AdjustedRoll = adjustedRoll};
        }

        private RollState()
        {
        }

        public List<int> ActualRoll { get; set; }
        public List<int> AdjustedRoll { get; set; }
        public int TargetNumber { get; set; }

        public int Result => AdjustedRoll.Max();
        public bool Win => Result >= TargetNumber;
        public int Successes => AdjustedRoll.Count(x => x >= TargetNumber);
    }
}

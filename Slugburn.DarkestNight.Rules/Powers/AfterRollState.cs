using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public class AfterRollState
    {
        public List<int> Roll { get; }

        public AfterRollState(List<int> roll)
        {
            Roll = roll;
        }

        public bool Repeat { get; set; }
    }
}
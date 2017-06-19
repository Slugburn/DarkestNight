using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes.Impl;

namespace Slugburn.DarkestNight.Rules
{
    public class Dice
    {
        public List<DiceDetail> Details { get; }

        public int Total
        {
            get
            {
                var total = Details.Sum(x => x.Modifier);
                return total > 0 ? total : 1;
            }
        }

        public Dice(List<DiceDetail> details)
        {
            Details = details;
        }
    }
}
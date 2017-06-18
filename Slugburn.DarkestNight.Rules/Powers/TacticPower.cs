using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights.Implementations;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public abstract class TacticPower : Power
    {
        public Func<int> GetBaseDieCount { get; protected set; }

        public TacticType TacticType { get; protected set; }

        protected TacticPower(TacticType tacticType, int dieCount) :this(tacticType)
        {
            GetBaseDieCount = ()=>dieCount;
        }

        protected TacticPower(TacticType tacticType)
        {
            Type = PowerType.Tactic;
            TacticType = tacticType;
        }

        public override bool IsUsable()
        {
            // Confusion blight prevents use of tactic powers
            return base.IsUsable() && !Hero.GetBlights().Any(x => x is Confusion);
        }
    }
}

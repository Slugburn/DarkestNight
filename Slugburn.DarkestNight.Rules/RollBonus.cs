using System;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules
{
    class RollBonus : IPowerEffect
    {
        public RollBonus(RollType rollType, BonusType bonusType, int amount, ISource source)
        {
            throw new System.NotImplementedException();
        }

        public bool Auto { get; set; }
        public System.Action OnUse { get; set; }
        public Func<IHero,bool> Restriction { get; set; }
        public bool Active { get; set; }
    }
}
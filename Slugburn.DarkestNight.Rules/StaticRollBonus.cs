using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules
{
    internal class StaticRollBonus : IRollModifier
    {
        public static StaticRollBonus Create(string name, RollType rollType, int dieCount)
        {
            return new StaticRollBonus {Name = name, RollType = rollType, DieCount = dieCount};
        }

        public int DieCount { get; set; }
        public string Name { get; set;  }
        public RollType RollType { get; set; }

        public int GetModifier(Hero hero, RollType rollType)
        {
            return rollType == RollType ?  DieCount : 0;
        }
    }
}
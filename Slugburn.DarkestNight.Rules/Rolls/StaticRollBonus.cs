using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Rolls
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
            return rollType == RollType || RollType == RollType.Any ?  DieCount : 0;
        }
    }
}
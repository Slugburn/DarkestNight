using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules
{
    internal class StaticRollBonus : IRollModifier
    {
        private readonly int _dieCount;
        public string Name { get; }
        public TacticType TacticType { get; }

        public StaticRollBonus(string name, TacticType tacticType, int dieCount)
        {
            _dieCount = dieCount;
            Name = name;
            TacticType = tacticType;
        }

        public int GetModifier(Hero hero)
        {
            return _dieCount;
        }
    }
}
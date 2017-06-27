using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Rolls
{
    public class PowerRollBonus : IRollModifier
    {
        private readonly IPower _power;
        private readonly RollType _rollType;
        private readonly int _dieCount;

        public PowerRollBonus(IPower power, RollType rollType, int dieCount)
        {
            _power = power;
            _rollType = rollType;
            _dieCount = dieCount;
            Name = power.Name;
        }

        public int GetModifier(Hero hero, RollType rollType)
        {
            if (_power.Exhausted) return 0;
            if (_rollType == RollType.Any) return _dieCount;
            return _rollType == rollType ? _dieCount : 0;
        }

        public string Name { get; }
    }
}

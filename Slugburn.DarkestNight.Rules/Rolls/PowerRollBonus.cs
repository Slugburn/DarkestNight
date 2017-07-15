using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Rolls
{
    public class PowerRollBonus : IModifier
    {
        private readonly IPower _power;
        private readonly ModifierType _modifierType;
        private readonly int _dieCount;

        public PowerRollBonus(IPower power, ModifierType modifierType, int dieCount)
        {
            _power = power;
            _modifierType = modifierType;
            _dieCount = dieCount;
            Name = power.Name;
        }

        public virtual int GetModifier(Hero hero, ModifierType modifierType)
        {
            if (_power.Exhausted) return 0;
            return _modifierType == modifierType ? _dieCount : 0;
        }

        public string Name { get; }
    }
}

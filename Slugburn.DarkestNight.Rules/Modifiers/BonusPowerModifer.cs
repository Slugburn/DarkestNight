using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Modifiers
{
    public abstract class BonusPowerModifer : IModifier
    {
        private readonly ModifierType _modifierType;

        protected BonusPowerModifer(IBonusPower power, ModifierType modifierType)
        {
            Power = power;
            _modifierType = modifierType;
        }

        protected IBonusPower Power { get; }

        public string Name => Power.Name;

        public int GetModifier(Hero hero, ModifierType modifierType)
        {
            if (modifierType != _modifierType || !Power.IsUsable(hero)) return 0;
            return GetAmount();
        }

        protected abstract int GetAmount();
    }
}
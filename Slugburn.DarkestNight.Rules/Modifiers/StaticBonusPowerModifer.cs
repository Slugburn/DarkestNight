using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Modifiers
{
    internal class StaticBonusPowerModifer : BonusPowerModifer
    {
        private readonly int _amount;

        public StaticBonusPowerModifer(IBonusPower power, ModifierType modifierType, int amount) 
            : base(power,modifierType)
        {
            _amount = amount;
        }

        protected override int GetAmount()
        {
            return _amount;
        }
    }
}
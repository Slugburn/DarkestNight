using Slugburn.DarkestNight.Rules.Powers;

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
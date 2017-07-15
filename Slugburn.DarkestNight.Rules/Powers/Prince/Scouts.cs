using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Powers.Prince
{
    internal class Scouts : ActivateOnSpacePower
    {
        public Scouts()
        {
            Name = "Scouts";
            Text = "Spend 1 Secrecy to activate in your location.";
            ActiveText = "Heroes gain +1d in searches there.";
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.CanSpendSecrecy;
        }

        protected override void PayActivationCost()
        {
            Owner.SpendSecrecy(1);
        }

        protected override void ActivateEffect()
        {
            Target.AddModifier(new PowerRollBonus(this, ModifierType.SearchDice, 1));
        }
    }
}
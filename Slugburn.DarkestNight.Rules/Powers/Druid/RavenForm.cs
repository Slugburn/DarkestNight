using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class RavenForm : DruidFormPower
    {
        public RavenForm()
        {
            Name = "Raven Form";
            Text = "Deactivate all Forms. Optionally activate.";
            ActiveText = "+1 die in searches. When you travel, you may move two spaces. You cannot gain Grace.";
        }

        public override void Activate(Hero hero)
        {
            base.Activate(Owner);
            Owner.AddModifier(new RavenFormTravelBonus(this));
            Owner.AddModifier(StaticRollBonus.Create(Name, ModifierType.SearchDice, 1));
        }

        public override bool Deactivate(Hero hero)
        {
            if (!base.Deactivate(hero)) return false;
            hero.RemoveModifiers(Name);
            return true;
        }

        internal class RavenFormTravelBonus : IModifier
        {
            private readonly IPower _power;

            public RavenFormTravelBonus(IPower power)
            {
                _power = power;
            }

            public int GetModifier(Hero hero, ModifierType modifierType)
            {
                return modifierType == ModifierType.TravelSpeed && !_power.IsExhausted ? 1: 0;
            }

            public string Name => _power.Name;
        }
    }
}
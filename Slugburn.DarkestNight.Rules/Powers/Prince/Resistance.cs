using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Powers.Prince
{
    internal class Resistance : ActivateOnSpacePower
    {
        public Resistance()
        {
            Name = "Resistance";
            StartingPower = true;
            Text = "Spend 1 Secrecy to activate in your location.";
            ActiveText = "Heroes gain +1d in fights when attacking blights there.";
        }

        protected override void PayActivationCost()
        {
            Owner.SpendSecrecy(1);
        }

        protected override void ActivateEffect()
        {
            Target.AddModifier(new ResistanceRollBonus(this));
        }

        internal class ResistanceRollBonus : PowerRollBonus
        {
            public ResistanceRollBonus(Resistance resistance) :base(resistance, ModifierType.FightDice, 1 ) { }

            public override int GetModifier(Hero hero, ModifierType modifierType)
            {
                var modifier = base.GetModifier(hero, modifierType);
                if (modifier == 0) return 0;
                return hero.ConflictState?.ConflictType == ConflictType.Attack ? modifier : 0;
            }
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.Secrecy > 0;
        }
    }
}
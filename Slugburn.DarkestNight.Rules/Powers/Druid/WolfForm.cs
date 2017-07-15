using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class WolfForm : DruidFormPower
    {
        private const string PowerName = "Wolf Form";

        public WolfForm()
        {
            Name = PowerName;
            Text = "Deactivate all Forms. Optionally activate.";
            ActiveText = "+1 die in fights. +1 die when eluding. You cannot gain Grace.";
        }

        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            hero.AddModifier(StaticRollBonus.Create(PowerName, ModifierType.FightDice, 1));
            hero.AddModifier(StaticRollBonus.Create(PowerName, ModifierType.EludeDice, 1));
        }

        public override bool Deactivate(Hero hero)
        {
            if (!base.Deactivate(hero)) return false;
            hero.RemoveModifiers(PowerName);
            return true;
        }
    }
}
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class TreeForm : DruidFormPower
    {
        private const string PowerName = "Tree Form";

        public TreeForm()
        {
            Name = PowerName;
            Text = "Deactivate all Forms. Optionally activate.";
            ActiveText = "Gain 2 Grace (up to default) at the start of your turn. Your actions can only be to hide or use a Druid power.";
        }

        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            hero.IsGraceGainBlocked = false;
            hero.Triggers.Add(HeroTrigger.StartedTurn, Name, new TreeFormStartTurnHandler());
            hero.AddActionFilter(PowerName, new[] { "Hide", "Tree Form", "Celerity", "Raven Form", "Sprite Form", "Wolf Form", "Deactivate Form" });
        }

        public override bool Deactivate(Hero hero)
        {
            if (!base.Deactivate(hero)) return false;
            hero.Triggers.Remove(HeroTrigger.StartedTurn, PowerName);
            hero.RemoveActionFilter(PowerName);
            return true;
        }

        private class TreeFormStartTurnHandler : ITriggerHandler<Hero>
        {
            public void HandleTrigger(Hero registrar, string source, TriggerContext context)
            {
                registrar.GainGrace(2, registrar.DefaultGrace);
            }
        }
    }
}
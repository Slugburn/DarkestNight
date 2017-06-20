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
            hero.CanGainGrace = true;
            hero.Triggers.Register(HeroTrigger.StartTurn, new TreeFormStartTurnHandler());
            hero.AddActionFilter(PowerName, HeroState.ChoosingAction, new[] { "Hide", "Tree Form", "Celerity", "Raven Form", "Sprite Form", "Wolf Form", "Deactivate Form" });
        }

        public override bool Deactivate(Hero hero)
        {
            if (!base.Deactivate(hero)) return false;
            hero.Triggers.Unregister(HeroTrigger.StartTurn, PowerName);
            hero.RemoveActionFilter(PowerName);
            return true;
        }

        private class TreeFormStartTurnHandler : ITriggerHandler<Hero>
        {
            public string Name => PowerName;
            public void HandleTrigger(Hero registrar, TriggerContext context)
            {
                registrar.GainGrace(2, registrar.DefaultGrace);
            }
        }
    }
}
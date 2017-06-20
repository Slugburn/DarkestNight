using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    class DarkVeil : Bonus
    {
        private const string PowerName = "Dark Veil";

        public DarkVeil()
        {
            Name = PowerName;
            StartingPower = true;
            Text =
                "Exhaust at any time to ignore blights' effects until your next turn. *OR* Exhaust after you fail an attack on a blight to ignore its Defense.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddAction(new DarkVeilAction());
            hero.Triggers.Register(HeroTrigger.BeforeBlightDefends, new FailedAttackHandler());
        }

        private class FailedAttackHandler : ITriggerHandler<Hero>
        {
            public string Name => PowerName;
            public void HandleTrigger(Hero registrar, TriggerContext context)
            {
                var hero = registrar;
                var power = hero.GetPower(Name);
                if (!power.IsUsable(hero)) return;
                if (!hero.Player.AskUsePower(Name, power.Text)) return;
                power.Exhaust(hero);
                context.Cancel = true;
            }
        }

        private class DarkVeilAction : PowerAction
        {
            public DarkVeilAction() : base(PowerName)
            {
            }

            public override void Act(Hero hero)
            {
                hero.Game.AddIgnoreBlight(IgnoreBlight.Create(Name, hero));
                hero.Triggers.Register(HeroTrigger.StartTurn, new DarkVeilEnds());
                hero.GetPower(Name).Exhaust(hero);
            }

            private class DarkVeilEnds : ITriggerHandler<Hero>
            {
                public string Name => PowerName;
                public void HandleTrigger(Hero registrar, TriggerContext context)
                {
                    var hero = registrar;
                    hero.Game.RemoveIgnoreBlight(Name);
                    hero.Triggers.Unregister(HeroTrigger.StartTurn, Name);
                }
            }
        }
    }
}
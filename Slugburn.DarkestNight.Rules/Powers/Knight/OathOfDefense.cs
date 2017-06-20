using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class OathOfDefense : Oath
    {
        private const string PowerName = "Oath of Defense";

        public OathOfDefense()
        {
            Name = PowerName;
            ActiveText = "Gain 1 Grace (up to default) at start of turn.";
            FulfillText = "No blights at location; You gain 1 Grace.";
            BreakText = "Leave location; you lose all Grace.";
        }

        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            // fullfill immediately if no blights are at the current location
            if (!hero.GetBlights().Any())
            {
                FulfillOath(hero);
            }
            else
            {
                hero.Triggers.Register(HeroTrigger.StartTurn, new OathOfDefenseActive() { Name = Name });
                hero.Game.Triggers.Register(GameTrigger.BlightDestroyed, new OathOfDefenseFulfilled { Name = Name, HeroName = hero.Name });
                hero.Triggers.Register(HeroTrigger.ChangeLocation, new OathOfDefenseBroken() { Name = Name });
            }
            hero.IsActionAvailable = false;
        }

        private class OathOfDefenseFulfilled : GameTriggerHandler
        {
            public string HeroName { get; set; }

            public override void HandleTrigger(Game game, TriggerContext context)
            {
                var location = context.GetState<Location>();
                var hero = game.GetHero(HeroName);
                if (location != hero.Location) return;
                var space = game.Board[location];
                if (space.Blights.Any()) return;
                var power = (OathOfDefense)hero.GetPower(PowerName);
                power.FulfillOath(hero);
            }
        }

        private void FulfillOath(Hero hero)
        {
            hero.GainGrace(1, int.MaxValue);
            Deactivate(hero);
        }

        private class OathOfDefenseActive : HeroTriggerHandler
        {
            public override void HandleTrigger(Hero registrar, TriggerContext context)
            {
                var hero = registrar;
                hero.GainGrace(1, hero.DefaultGrace);
            }
        }
        private class OathOfDefenseBroken : HeroTriggerHandler
        {
            public override void HandleTrigger(Hero hero, TriggerContext context)
            {
                hero.LoseGrace(hero.Grace);
                var power = (OathOfDefense)hero.GetPower(Name);
                power.Deactivate(hero);
            }
        }
    }
}
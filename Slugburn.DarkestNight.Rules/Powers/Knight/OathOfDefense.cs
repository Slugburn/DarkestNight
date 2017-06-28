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
                Fulfill(hero);
            }
            else
            {
                hero.Triggers.Add(HeroTrigger.StartTurn, Name, new OathOfDefenseActive());
                hero.Game.Triggers.Add(GameTrigger.BlightDestroyed, Name, new OathOfDefenseFulfilled { HeroName = hero.Name });
                hero.Triggers.Add(HeroTrigger.Moved, Name, new OathOfDefenseBroken());
            }
            hero.IsActionAvailable = false;
        }

        private class OathOfDefenseFulfilled : ITriggerHandler<Game>
        {
            public string HeroName { get; set; }

            public void HandleTrigger(Game game, string source, TriggerContext context)
            {
                var location = context.GetState<Location>();
                var hero = game.GetHero(HeroName);
                if (location != hero.Location) return;
                var space = game.Board[location];
                if (space.Blights.Any()) return;
                var power = (IOath)hero.GetPower(PowerName);
                power.Fulfill(hero);
            }
        }

        private class OathOfDefenseActive : ITriggerHandler<Hero>
        {
            public void HandleTrigger(Hero registrar, string source, TriggerContext context)
            {
                var hero = registrar;
                hero.GainGrace(1, hero.DefaultGrace);
            }
        }
        private class OathOfDefenseBroken : ITriggerHandler<Hero>
        {
            public void HandleTrigger(Hero hero, string source, TriggerContext context)
            {
                var oath = (IOath)hero.GetPower(source);
                oath.Break(hero);
            }
        }

        public override void Fulfill(Hero hero)
        {
            hero.GainGrace(1, int.MaxValue);
            Deactivate(hero);
        }

        public override void Break(Hero hero)
        {
            hero.LoseGrace(hero.Grace);
            Deactivate(hero);
        }
    }
}
using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players.Models;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Benediction : ActionPower
    {
        private const string PowerName = "Benediction";

        public Benediction()
        {
            Name = PowerName;
            StartingPower = true;
            Text = "One hero at your location gains 1 Grace (up to default). If they now have more Grace than you, you gain 1 Grace.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddAction(new BenedictionAction());
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) &&
                // any heroes at the priest's location that can be healed?
                   hero.Game.Heroes.Any(target => IsValidTarget(hero, target));
        }

        private static bool IsValidTarget(Hero hero, Hero target)
        {
            return target.Location == hero.Location && target.Grace < target.DefaultGrace && target.CanGainGrace;
        }

        internal class BenedictionAction : PowerAction
        {
            public BenedictionAction() : base(PowerName)
            {
            }

            public override void Act(Hero hero)
            {
                if (!IsAvailable(hero))
                    throw new PowerNotUsableException(_powerName);

                var validTargets = hero.Game.Heroes.Where(target => IsValidTarget(hero, target)).ToList();

                hero.SetHeroSelectionHandler(new BenedictionHeroSelection());

                var view = new PlayerHeroSelection(validTargets.Select(PlayerHero.FromHero));
                hero.Player.DisplayHeroSelection(view);
            }
        }

        internal class BenedictionHeroSelection : IHeroSelectionHandler
        {
            public void Handle(Hero hero, Hero selectedHero)
            {
                if (!IsValidTarget(hero, selectedHero))
                    throw new InvalidOperationException($"{selectedHero.Name} is not a valid target for {PowerName}.");
                selectedHero.GainGrace(1, selectedHero.DefaultGrace);
                if (selectedHero.Grace > hero.Grace)
                    hero.GainGrace(1, int.MaxValue);
                hero.IsActionAvailable = false;
            }
        }
    }
}
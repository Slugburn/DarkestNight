﻿using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;

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

        protected override void OnLearn()
        {
            Owner.AddAction(new BenedictionAction(this));
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) &&
                // any heroes at the priest's location that can be healed?
                   hero.Game.Heroes.Any(target => IsValidTarget(hero, target));
        }

        private static bool IsValidTarget(Hero hero, Hero target)
        {
            return target.Location == hero.Location && target.Grace < target.DefaultGrace && target.CanGainGrace();
        }

        internal class BenedictionAction : PowerAction, ICallbackHandler<Hero>
        {
            public BenedictionAction(IActionPower power) : base(power)
            {
            }

            public override void Execute(Hero hero)
            {
                if (!IsAvailable(hero))
                    throw new CommandNotAvailableException(hero, this);

                var validTargets = hero.Game.Heroes.Where(target => IsValidTarget(hero, target)).ToList();

                var view = new HeroSelectionModel(validTargets);
                hero.Player.DisplayHeroSelection(view, Callback.For(hero, this));
            }

            public void HandleCallback(Hero hero, Hero data)
            {
                var selectedHero = data;
                if (!IsValidTarget(hero, selectedHero))
                    throw new InvalidOperationException($"{selectedHero.Name} is not a valid target for {PowerName}.");
                selectedHero.GainGrace(1, selectedHero.DefaultGrace);
                if (selectedHero.Grace > hero.Grace)
                    hero.GainGrace(1, int.MaxValue);
            }
        }
    }
}
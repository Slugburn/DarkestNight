﻿using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class PowerContext : HeroContext, IPowerContext
    {
        private readonly Hero _hero;
        private readonly IPower _power;

        public PowerContext(Game game, FakePlayer player, Hero hero, IPower power) :base(game, player, hero)
        {
            _hero = game.ActingHero;
            _power = power;
        }

        public IPowerContext IsActive(string target = null)
        {
            var targetable = _power as ITargetable;
            targetable?.SetTarget(target);
            var activatable = _power as IActivateable;
            activatable?.Activate(_hero);
            _hero.IsActionAvailable = true;
            return this;
        }

        public IPowerContext IsExhausted(bool exhausted = true)
        {
            if (exhausted)
                _power.Exhaust(_hero);
            else
                _power.Refresh();
            return this;
        }
    }
}
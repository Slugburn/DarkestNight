﻿using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class PowerContext : GivenContext, IPowerContext
    {
        private readonly Hero _hero;
        private readonly IPower _power;

        public PowerContext(Game game, FakePlayer player, IPower power) :base(game, player)
        {
            _hero = game.ActingHero;
            _power = power;
        }

        public IPowerContext IsActive()
        {
            ((IActivateable) _power).Activate(_hero);
            _hero.IsActionAvailable = true;
            return this;
        }

        public IPowerContext IsExhausted()
        {
            _power.Exhaust(_hero);
            return this;
        }
    }
}
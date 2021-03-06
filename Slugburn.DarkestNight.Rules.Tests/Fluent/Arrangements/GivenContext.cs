﻿using System;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class GivenContext : TestRoot, IGiven
    {
        public GivenContext(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IGameContext Game => new GameContext(base.GetGame(), GetPlayer());

        public ILocationContext Location(string location)
        {
            var space = base.GetGame().Board[location.ToEnum<Location>()];
            return new LocationContext(base.GetGame(), GetPlayer(), space);
        }

        public IHeroContext Hero(string heroName = null)
        {
            return new HeroContext(GetGame(), GetPlayer(), GetHero(heroName));
        }
    }
}
using System;
using Slugburn.DarkestNight.Rules.Extensions;
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

        public IPowerContext Power(string powerName)
        {
            var power = GetGame().GetPower(powerName);
            return new PowerContext(GetGame(), GetPlayer(), power);
        }

        public IGiven ActingHero(Action<HeroContext> def)
        {
            var hero = base.GetGame().ActingHero;
            var ctx = new HeroContext(hero);
            def(ctx);
            return this;
        }
    }
}
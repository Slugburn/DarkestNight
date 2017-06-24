using System;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class ThenContext : TestRoot, IThen
    {
        public ThenContext(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IPlayerExpectation Player => new PlayerExpectation(base.GetGame(), base.GetPlayer());

        public IThen Hero(Action<HeroExpectation> expect)
        {
            var expectation = new HeroExpectation(base.GetGame().ActingHero);
            expect(expectation);
            expectation.Verify();
            return this;
        }

        public IGameExpectation Game => new GameExpectation(base.GetGame(), base.GetPlayer());

        public IThen Location(string location, Action<LocationExpectation> expect)
        {
            var space = base.GetGame().Board[location.ToEnum<Location>()];
            var expectation = new LocationExpectation(space);
            expect(expectation);
            expectation.Verify();
            return this;
        }
    }
}
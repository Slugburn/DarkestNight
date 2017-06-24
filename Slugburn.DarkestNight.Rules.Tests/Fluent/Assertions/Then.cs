using System;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class Then : TestRoot, IThen
    {
        public Then(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IPlayerExpectation Player => new PlayerExpectation(_game, _player);

        public IThen Hero(Action<HeroExpectation> expect)
        {
            var expectation = new HeroExpectation(_game.ActingHero);
            expect(expectation);
            expectation.Verify();
            return this;
        }

        public IThen Game(Action<GameExpectation> expect)
        {
            var expection = new GameExpectation(_game);
            expect(expection);
            return this;
        }

        public IThen Location(string location, Action<LocationExpectation> expect)
        {
            var space = _game.Board[location.ToEnum<Location>()];
            var expectation = new LocationExpectation(space);
            expect(expectation);
            expectation.Verify();
            return this;
        }
    }
}
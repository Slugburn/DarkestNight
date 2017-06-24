using Shouldly;
using Slugburn.DarkestNight.Rules.Extensions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class GameExpectation
    {
        private readonly Game _game;

        public GameExpectation(Game game)
        {
            _game = game;
        }

        public GameExpectation NecromancerLocation(string location)
        {
            _game.Necromancer.Location.ShouldBe(location.ToEnum<Location>());
            return this;
        }

        public GameExpectation Darkness(int darkness)
        {
            _game.Darkness.ShouldBe(darkness);
            return this;
        }

        public GameExpectation EventDeckIsReshuffled()
        {
            _game.Events.Count.ShouldBe(33);
            return this;
        }
    }
}
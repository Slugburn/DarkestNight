using Shouldly;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class GameExpectation : ThenContext, IGameExpectation
    {
        public GameExpectation(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IGameExpectation NecromancerLocation(string location)
        {
            GetGame().Necromancer.Location.ShouldBe(location.ToEnum<Location>());
            return this;
        }

        public IGameExpectation Darkness(int darkness)
        {
            GetGame().Darkness.ShouldBe(darkness);
            return this;
        }

        public IGameExpectation EventDeckIsReshuffled()
        {
            GetGame().Events.Count.ShouldBe(33);
            return this;
        }
    }
}
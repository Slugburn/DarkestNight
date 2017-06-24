using Shouldly;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class GameExpectation : ThenContext, IGameExpectation
    {
        public GameExpectation(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IGameExpectation Darkness(int darkness)
        {
            GetGame().Darkness.ShouldBe(darkness);
            return this;
        }
    }
}
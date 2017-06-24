using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class ThenContext : TestRoot, IThen
    {
        public ThenContext(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IPlayerExpectation Player => new PlayerExpectation(base.GetGame(), base.GetPlayer());

        public IGameExpectation Game => new GameExpectation(base.GetGame(), base.GetPlayer());
    }
}
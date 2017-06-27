using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public class TestScenario : IFakeContext
    {
        static TestScenario()
        {
            Die.Implementation = new FakeDie();
        }

        public static GivenContext Given => CreateRootGiven();

        private static GivenContext CreateRootGiven()
        {
            var game = new Game();
            var player = new FakePlayer(game);
            game.AddPlayer(player);
            var given = new GivenContext(game, player);
            return given;
        }
    }
}
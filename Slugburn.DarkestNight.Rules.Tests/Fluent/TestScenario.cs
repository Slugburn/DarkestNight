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

        public static IGameContext Game => CreateGameContext();

        private static IGameContext CreateGameContext()
        {
            var game = new Game();
            var player = new FakePlayer(game);
            game.AddPlayer(player);
            var context = new GameContext(game, player);
            return context;
        }
    }
}
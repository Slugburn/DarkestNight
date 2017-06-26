using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class PlayerExpectation : ThenContext, IPlayerExpectation
    {
        public PlayerExpectation(Game game, FakePlayer player) : base(game, player)
        {
        }

        public void Verify()
        {
        }
    }
}
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Conflicts
{
    [TestFixture]
    public class ConflictTest
    {
        [TestCase("Scout")]
        [TestCase("Flying Demon")]
        public void CanNotFight(string enemyName)
        {
            TestScenario.Game
                .WithHero().IsFacingEnemy(enemyName)
                .Then(Verify.Player.ConflictModel.HasTactics("Elude"));
        }

        [TestCase("Vile Messenger")]
        public void CanNotElude(string enemyName)
        {
            TestScenario.Game
                .WithHero().IsFacingEnemy(enemyName)
                .Then(Verify.Player.ConflictModel.HasTactics("Fight"));
        }
    }
}

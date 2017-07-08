using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Blights
{
    [TestFixture]
    public class ConfusionTest
    {
        [Test]
        public void Effect()
        {
            // Effect: While a hero is in the affected location, his Tactic powers have no effect.
            TestScenario.Game
                .WithHero("Knight").At("Village").HasPowers("Charge", "Reckless Abandon", "Sprint")
                .Location("Village").HasBlights("Confusion")
                .Given.Hero().IsFacingEnemy("Skeleton")
                .Then(Verify.Player.ConflictModel.HasTactics("Fight", "Elude"));
        }

        [Test]
        public void Defense()
        {
            // Defense: Lose a turn.
            TestScenario.Game
                .WithHero().At("Village")
                .Location("Village").HasBlights("Confusion")
                .When.Player.TakesAction("Attack").Fights(Fake.Rolls(1))
                .Given.Game.NewDay()
                .Then(Verify.Hero().CanTakeAction("Start Turn", false));
        }
    }
}

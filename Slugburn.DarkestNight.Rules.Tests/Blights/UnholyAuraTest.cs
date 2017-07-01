using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Blights
{
    [TestFixture]
    public class UnholyAuraTest
    {
        [Test]
        public void Effect()
        {
            // While a hero is in the affected location, he rolls one fewer die when fighting (to a minimum of 1).
            TestScenario.Game
                .WithHero("Knight").At("Village").HasPowers("Reckless Abandon")
                .Location("Village").HasBlights("UnholyAura", "UnholyAura")
                .Given.Hero().IsFacingEnemy("Skeleton")
                .When.Player.CompletesConflict("Skeleton", "Reckless Abandon")
                .Then(Verify.Hero().RolledNumberOfDice(2));
        }

        [Test]
        public void Defense()
        {
            // Lose 1 Grace.
            TestScenario.Game
                .WithHero().At("Village")
                .Location("Village").HasBlights("UnholyAura")
                .When.Player.TakesAction("Attack").Fights(Fake.Rolls(1))
                .Then(Verify.Hero().LostGrace());
        }
    }
}
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Blights
{
    [TestFixture]
    public class ShroudTest
    {
        [Test]
        public void Effect()
        {
            // Other types of blights at the location of a Shroud cannot be destroyed (the Shroud must be destroyed first.)
            TestScenario.Game
                .WithHero().At("Village")
                .Location("Village").HasBlights("Desecration", "Shroud")
                .When.Player.TakesAction("Attack").Targets("Desecration").ResolvesConflict().AcceptsRoll()
                .Then(Verify.Location("Village").Blights("Desecration", "Shroud"));
        }

        [Test]
        public void Effect_MultipleTargets()
        {
            TestScenario.Game
                .WithHero("Acolyte").HasPowers("Call to Death").At("Swamp")
                .Given.Location("Swamp").HasBlights("Desecration", "Shroud")
                .When.Player.TakesAction("Call to Death").Targets("Desecration", "Shroud").UsesTactic("Fight").ResolvesConflict(Fake.Rolls(5, 6))
                .Then(Verify.Player.ConflictModel
                    .Rolled(5, 6)
                    .SelectedTarget("Desecration").Target(4).Result(5, "Destroy blight.")
                    .SelectedTarget("Shroud").Target(5).Result(6, "Destroy blight."))
                .When.Player.AcceptsRoll()
                .Then(Verify.Location("Swamp").Blights());
        }

        [Test]
        public void Defense()
        {
            // Wound.
            TestScenario.Game
                .WithHero().At("Village")
                .Location("Village").HasBlights("Shroud")
                .When.Player.TakesAction("Attack").Fights(Fake.Rolls(1))
                .Then(Verify.Hero().HasUsedAction().WasWounded());
        }
    }
}

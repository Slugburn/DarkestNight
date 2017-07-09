using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class GuardedTroveTest
    {
        [TestCase("Fight")]
        [TestCase("Elude")]
        public void Win(string tactic)
        {
            TestScenario
                .Game.WithHero()
                .Given.Hero().HasDrawnEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Player.ConflictModel.HasTargets("Guarded Trove"))
                .Given.Game.NextSearchResult(Find.Waystone)
                .When.Player.CompletesConflict("Guarded Trove", tactic, Fake.Rolls(6))
                .Then(Verify.Player.SearchView.Results("Waystone"))
                .Then(Verify.Player.ConflictModel.IsHidden())
                .When.Player.SelectsSearchResult()
                .Then(Verify.Player.ConflictModel.IsHidden())
                .Then(Verify.Player.Hero().LostSecrecy(1).Inventory("Waystone").Commands.Includes("Waystone"));
        }

        [Test]
        public void FailElude_DrawEvent()
        {
            TestScenario
                .Game.WithHero()
                .Given.Hero().HasDrawnEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Player.ConflictModel.HasTargets("Guarded Trove"))
                .When.Player.Eludes(Fake.Rolls(5)) 
                .Then(Verify.Player.EventView.HasOptions("Spend Secrecy", "Draw Event"))
                .When.Player.SelectsEventOption("Draw Event")
                .Then(Verify.Hero().HasUnresolvedEvents(1));
        }

        [Test]
        public void FailElude_LostSecrecy()
        {
            TestScenario
                .Game.WithHero()
                .Given.Hero().HasDrawnEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Player.ConflictModel.HasTargets("Guarded Trove"))
                .When.Player.Eludes(Fake.Rolls(5))
                .Then(Verify.Player.EventView.HasOptions("Spend Secrecy", "Draw Event"))
                .When.Player.SelectsEventOption("Spend Secrecy")
                .Then(Verify.Hero().LostSecrecy());
        }

        [Test]
        public void FailElude_NoSecrecy()
        {
            TestScenario
                .Game.WithHero().Secrecy(0)
                .Given.Hero().HasDrawnEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Player.ConflictModel.HasTargets("Guarded Trove"))
                .When.Player.Eludes(Fake.Rolls(5))
                .Then(Verify.Player.EventView.HasOptions("Draw Event"));
        }

        [Test]
        public void FailFight()
        {
            TestScenario
                .Game.WithHero()
                .Given.Hero().HasDrawnEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Player.ConflictModel.HasTargets("Guarded Trove"))
                .When.Player.Fights(Fake.Rolls(5))
                .Then(Verify.Hero().WasWounded());
        }
    }
}
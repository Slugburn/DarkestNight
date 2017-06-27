using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
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
                .Given.ActingHero().DrawsEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Player.ConflictView.HasTargets("Guarded Trove"))
                .Given.Game.NextSearchResult(Find.Waystone)
                .When.Player.CompletesConflict("Guarded Trove", tactic, Fake.Rolls(6))
                .Then(Verify.Hero.LostSecrecy().HasItems("Waystone"));
        }

        [Test]
        public void FailElude_DrawEvent()
        {
            TestScenario
                .Game.WithHero()
                .Given.ActingHero().DrawsEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Player.ConflictView.HasTargets("Guarded Trove"))
                .When.Player.Eludes(Fake.Rolls(5)) 
                .Then(Verify.Player.EventView.HasOptions("Spend Secrecy", "Draw Event"))
                .When.Player.SelectsEventOption("Draw Event")
                .Then(Verify.Hero.HasUnresolvedEvents(1));
        }

        [Test]
        public void FailElude_LostSecrecy()
        {
            TestScenario
                .Game.WithHero()
                .Given.ActingHero().DrawsEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Player.ConflictView.HasTargets("Guarded Trove"))
                .When.Player.Eludes(Fake.Rolls(5))
                .Then(Verify.Player.EventView.HasOptions("Spend Secrecy", "Draw Event"))
                .When.Player.SelectsEventOption("Spend Secrecy")
                .Then(Verify.Hero.LostSecrecy());
        }

        [Test]
        public void FailElude_NoSecrecy()
        {
            TestScenario
                .Game.WithHero().Secrecy(0)
                .Given.ActingHero().DrawsEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Player.ConflictView.HasTargets("Guarded Trove"))
                .When.Player.Eludes(Fake.Rolls(5))
                .Then(Verify.Player.EventView.HasOptions("Draw Event"));
        }

        [Test]
        public void FailFight()
        {
            TestScenario
                .Game.WithHero()
                .Given.ActingHero().DrawsEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Player.ConflictView.HasTargets("Guarded Trove"))
                .When.Player.Fights(Fake.Rolls(5))
                .Then(Verify.Hero.WasWounded());
        }
    }
}
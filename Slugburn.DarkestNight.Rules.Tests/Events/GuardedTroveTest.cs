using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

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
                .Given.Game.WithHero()
                .When.Hero.DrawsEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then().Player.Conflict(c => c.Target("Guarded Trove"))
                .When.Player.ResolvesConflict(c => c.Tactic(tactic).Target("Guarded Trove").Rolls(6))
                .Given.Game.NextSearchResult(Find.Waystone)
                .When.Player.AcceptsRoll()
                .Then(Verify.Hero.LostSecrecy().HasItems("Waystone"));
        }

        [Test]
        public void FailElude_DrawEvent()
        {
            TestScenario
                .Given.Game.WithHero()
                .When.Hero.DrawsEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then().Player.Conflict(c => c.Target("Guarded Trove"))
                .When.Player.ResolvesConflict(c => c.Tactic("Elude").Target("Guarded Trove").Rolls(5)).AcceptsRoll()
                .Then().Player.EventView.HasOptions("Spend Secrecy", "Draw Event")
                .When.Player.SelectsEventOption("Draw Event")
                .Then(Verify.Hero.HasUnresolvedEvents(1));
        }

        [Test]
        public void FailElude_LostSecrecy()
        {
            TestScenario
                .Given.Game.WithHero()
                .When.Hero.DrawsEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue").Then().Player.Conflict(c => c.Target("Guarded Trove"))
                .When.Player.ResolvesConflict(c => c.Tactic("Elude").Target("Guarded Trove").Rolls(5)).AcceptsRoll()
                .Then().Player.EventView.HasOptions("Spend Secrecy", "Draw Event")
                .When.Player.SelectsEventOption("Spend Secrecy")
                .Then(Verify.Hero.LostSecrecy());
        }

        [Test]
        public void FailElude_NoSecrecy()
        {
            TestScenario
                .Given.Game.WithHero().Secrecy(0)
                .When.Hero.DrawsEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then().Player.Conflict(c => c.Target("Guarded Trove"))
                .When.Player.ResolvesConflict(c => c.Tactic("Elude").Target("Guarded Trove").Rolls(5)).AcceptsRoll()
                .Then().Player.EventView.HasOptions("Draw Event");
        }

        [Test]
        public void FailFight()
        {
            TestScenario
                .Given.Game.WithHero()
                .When.Hero.DrawsEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then().Player.Conflict(c => c.Target("Guarded Trove"))
                .When.Player.ResolvesConflict(c => c.Tactic("Fight").Target("Guarded Trove").Rolls(5)).AcceptsRoll()
                .Then(Verify.Hero.WasWounded());
        }
    }
}
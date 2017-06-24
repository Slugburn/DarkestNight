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
                .Given.Game.Hero()
                .When.Hero.DrawsEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then.Player.Conflict(c => c.Target("Guarded Trove"))
                .When.Player.ResolvesConflict(c => c.Tactic(tactic).Target("Guarded Trove").Rolls(6))
                .Given.Game.NextSearchResult(Find.Waystone)
                .When.Player.AcceptsRoll()
                .Then.Hero(h => h.LostSecrecy().HasItem("Waystone"));
        }

        [Test]
        public void FailElude_DrawEvent()
        {
            TestScenario
                .Given.Game.Hero()
                .When.Hero.DrawsEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then.Player.Conflict(c => c.Target("Guarded Trove"))
                .When.Player.ResolvesConflict(c => c.Tactic("Elude").Target("Guarded Trove").Rolls(5)).AcceptsRoll()
                .Then.Player.Event.HasOptions("Spend Secrecy", "Draw Event")
                .When.Player.SelectsEventOption("Draw Event")
                .Then.Hero(h => h.Event(e => e.HasOutstanding(1)));
        }

        [Test]
        public void FailElude_LostSecrecy()
        {
            TestScenario
                .Given.Game.Hero()
                .When.Hero.DrawsEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then.Player.Conflict(c => c.Target("Guarded Trove"))
                .When.Player.ResolvesConflict(c => c.Tactic("Elude").Target("Guarded Trove").Rolls(5)).AcceptsRoll()
                .Then.Player.Event.HasOptions("Spend Secrecy", "Draw Event")
                .When.Player.SelectsEventOption("Spend Secrecy")
                .Then.Hero(h => h.LostSecrecy());
        }

        [Test]
        public void FailElude_NoSecrecy()
        {
            TestScenario
                .Given.Game.Hero(h => h.Secrecy(0))
                .When.Hero.DrawsEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then.Player.Conflict(c => c.Target("Guarded Trove"))
                .When.Player.ResolvesConflict(c => c.Tactic("Elude").Target("Guarded Trove").Rolls(5)).AcceptsRoll()
                .Then.Player.Event.HasOptions("Draw Event");
        }

        [Test]
        public void FailFight()
        {
            TestScenario
                .Given.Game.Hero()
                .When.Hero.DrawsEvent("Guarded Trove")
                .When.Player.SelectsEventOption("Continue")
                .Then.Player.Conflict(c => c.Target("Guarded Trove"))
                .When.Player.ResolvesConflict(c => c.Tactic("Fight").Target("Guarded Trove").Rolls(5)).AcceptsRoll()
                .Then.Hero(h => h.WasWounded());
        }
    }
}
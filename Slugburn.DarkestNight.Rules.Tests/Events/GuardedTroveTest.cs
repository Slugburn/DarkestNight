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
                .Given.Game(g => g.Hero())
                .When.Hero(x => x.DrawsEvent("Guarded Trove"))
                .When.Player(p => p.SelectsEventOption("Continue"))
                .Then.Player(p => p.Conflict(c => c.Target("Guarded Trove")))
                .When.Player(p => p.ResolvesConflict(c => c.Tactic(tactic).Target("Guarded Trove").Rolls(6)))
                .Given.Game(g => g.NextSearchResult(Find.Waystone))
                .When.Player(p => p.AcceptsRoll())
                .Then.Hero(h => h.LostSecrecy().HasItem("Waystone"));
        }

        [Test]
        public void FailFight()
        {
            TestScenario
                .Given.Game(g => g.Hero())
                .When.Hero(x => x.DrawsEvent("Guarded Trove"))
                .When.Player(p => p.SelectsEventOption("Continue"))
                .Then.Player(p => p.Conflict(c => c.Target("Guarded Trove")))
                .When.Player(p => p.ResolvesConflict(c => c.Tactic("Fight").Target("Guarded Trove").Rolls(5)).AcceptsRoll())
                .Then.Hero(h => h.WasWounded());
        }

        [Test]
        public void FailElude_LostSecrecy()
        {
            TestScenario
                .Given.Game(g=>g.Hero())
                .When.Hero(x => x.DrawsEvent("Guarded Trove"))
                .When.Player(p => p.SelectsEventOption("Continue"))
                .Then.Player(p => p.Conflict(c => c.Target("Guarded Trove")))
                .When.Player(p => p.ResolvesConflict(c => c.Tactic("Elude").Target("Guarded Trove").Rolls(5)).AcceptsRoll())
                .Then.Player(p => p.Event(e => e.HasOptions("Spend Secrecy", "Draw Event")))
                .When.Player(p => p.SelectsEventOption("Spend Secrecy"))
                .Then.Hero(h => h.LostSecrecy());
        }

        [Test]
        public void FailElude_DrawEvent()
        {
            TestScenario
                .Given.Game(g => g.Hero())
                .When.Hero(x => x.DrawsEvent("Guarded Trove"))
                .When.Player(p => p.SelectsEventOption("Continue"))
                .Then.Player(p => p.Conflict(c => c.Target("Guarded Trove")))
                .When.Player(p => p.ResolvesConflict(c => c.Tactic("Elude").Target("Guarded Trove").Rolls(5)).AcceptsRoll())
                .Then.Player(p => p.Event(e => e.HasOptions("Spend Secrecy", "Draw Event")))
                .When.Player(p => p.SelectsEventOption("Draw Event"))
                .Then.Hero(h => h.Event(e => e.HasOutstanding(1)));
        }

        [Test]
        public void FailElude_NoSecrecy()
        {
            TestScenario
                .Given.Game(g => g.Hero(h=>h.Secrecy(0)))
                .When.Hero(x => x.DrawsEvent("Guarded Trove"))
                .When.Player(p => p.SelectsEventOption("Continue"))
                .Then.Player(p => p.Conflict(c => c.Target("Guarded Trove")))
                .When.Player(p => p.ResolvesConflict(c => c.Tactic("Elude").Target("Guarded Trove").Rolls(5)).AcceptsRoll())
                .Then.Player(p => p.Event(e => e.HasOptions("Draw Event")));
        }
    }
}

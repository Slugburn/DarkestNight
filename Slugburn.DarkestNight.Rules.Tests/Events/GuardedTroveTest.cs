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
            new TestScenario()
                .GivenHero()
                .WhenHero(x => x.DrawsEvent("Guarded Trove"))
                .WhenPlayer(p => p.SelectsEventOption("Continue"))
                .ThenPlayer(p => p.Conflict(c => c.Target("Guarded Trove")))
                .WhenPlayer(p => p.ResolvesConflict(c => c.Tactic(tactic).Target("Guarded Trove").Rolls(6)))
                .GivenNextSearchResult(Find.Waystone)
                .WhenPlayer(p => p.AcceptsRoll())
                .ThenHero(h => h.LostSecrecy().HasItem("Waystone"));
        }

        [Test]
        public void FailFight()
        {
            new TestScenario()
                .GivenHero()
                .WhenHero(x => x.DrawsEvent("Guarded Trove"))
                .WhenPlayer(p => p.SelectsEventOption("Continue"))
                .ThenPlayer(p => p.Conflict(c => c.Target("Guarded Trove")))
                .WhenPlayer(p => p.ResolvesConflict(c => c.Tactic("Fight").Target("Guarded Trove").Rolls(5)).AcceptsRoll())
                .ThenHero(h => h.WasWounded());
        }

        [Test]
        public void FailElude_LostSecrecy()
        {
            new TestScenario()
                .GivenHero()
                .WhenHero(x => x.DrawsEvent("Guarded Trove"))
                .WhenPlayer(p => p.SelectsEventOption("Continue"))
                .ThenPlayer(p => p.Conflict(c => c.Target("Guarded Trove")))
                .WhenPlayer(p => p.ResolvesConflict(c => c.Tactic("Elude").Target("Guarded Trove").Rolls(5)).AcceptsRoll())
                .ThenPlayer(p => p.Event(e => e.HasOptions("Spend Secrecy", "Draw Event")))
                .WhenPlayer(p => p.SelectsEventOption("Spend Secrecy"))
                .ThenHero(h => h.LostSecrecy());
        }

        [Test]
        public void FailElude_DrawEvent()
        {
            new TestScenario()
                .GivenHero()
                .WhenHero(x => x.DrawsEvent("Guarded Trove"))
                .WhenPlayer(p => p.SelectsEventOption("Continue"))
                .ThenPlayer(p => p.Conflict(c => c.Target("Guarded Trove")))
                .WhenPlayer(p => p.ResolvesConflict(c => c.Tactic("Elude").Target("Guarded Trove").Rolls(5)).AcceptsRoll())
                .ThenPlayer(p => p.Event(e => e.HasOptions("Spend Secrecy", "Draw Event")))
                .WhenPlayer(p => p.SelectsEventOption("Draw Event"))
                .ThenHero(h => h.HasOutstandingEvents(1));
        }

        [Test]
        public void FailElude_NoSecrecy()
        {
            new TestScenario()
                .GivenHero(h=>h.Secrecy(0))
                .WhenHero(x => x.DrawsEvent("Guarded Trove"))
                .WhenPlayer(p => p.SelectsEventOption("Continue"))
                .ThenPlayer(p => p.Conflict(c => c.Target("Guarded Trove")))
                .WhenPlayer(p => p.ResolvesConflict(c => c.Tactic("Elude").Target("Guarded Trove").Rolls(5)).AcceptsRoll())
                .ThenPlayer(p => p.Event(e => e.HasOptions("Draw Event")));
        }
    }
}

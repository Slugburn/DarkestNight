using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class SloppySearchTest
    {
        [Test]
        public void SloppySearch_GainSecrecy()
        {
            TestScenario
                .Given.Game(g => g.Hero(h => h.Secrecy(0)))
                .When.Hero(h => h.DrawsEvent("Sloppy Search"))
                .Then.Player(p => p.Event(e => e.HasBody("Sloppy Search", 2, "Roll 1d and take the highest").HasOptions("Roll")))
                .When.Player(p => p.SelectsEventOption("Roll", x => x.Rolls(6)))
                .Then.Player(p => p.Event(e => e.ActiveRow("Gain 1 Secrecy").HasOptions("Gain Secrecy")))
                .When.Player(p => p.SelectsEventOption("Gain Secrecy"))
                .Then.Hero(h => h.Secrecy(1).Event(e=>e.HasOutstanding(0)));
        }

        [TestCase(5)]
        [TestCase(4)]
        public void SloppySearch_NoEffect(int roll)
        {
            TestScenario
                .Given.Game(g => g.Hero())
                .When.Hero(h => h.DrawsEvent("Sloppy Search"))
                .When.Player(p => p.SelectsEventOption("Roll", x=>x.Rolls(roll)))
                .Then.Player(p => p.Event(e => e.ActiveRow("No effect").HasOptions("No Effect")))
                .When.Player(p => p.SelectsEventOption("No Effect"))
                .Then.Hero(h => h.Event(e=>e.HasOutstanding(0)));
        }

        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public void SloppySearch_SpendGrace(int roll)
        {
            TestScenario
                .Given.Game(g => g.Hero())
                .When.Hero(h => h.DrawsEvent("Sloppy Search"))
                .When.Player(p => p.SelectsEventOption("Roll", x=>x.Rolls(roll)))
                .Then.Player(p => p.Event(e => e.ActiveRow("Spend 1 Grace or lose 1 Secrecy").HasOptions("Spend Grace", "Lose Secrecy")))
                .When.Player(p => p.SelectsEventOption("Spend Grace"))
                .Then.Hero(h => h.LostGrace().Event(e=>e.HasOutstanding(0)));
        }

        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public void SloppySearch_LoseSecrecy(int roll)
        {
            TestScenario
                .Given.Game(g => g.Hero())
                .When.Hero(h => h.DrawsEvent("Sloppy Search"))
                .When.Player(p => p.SelectsEventOption("Roll", x => x.Rolls(roll)))
                .When.Player(p => p.SelectsEventOption("Lose Secrecy"))
                .Then.Hero(h => h.LostSecrecy().Event(e => e.HasOutstanding(0)));
        }

        [Test]
        public void SloppySearch_NoGraceToSpend()
        {
            TestScenario
                .Given.Game(g => g.Hero(h=>h.Grace(0)))
                .When.Hero(h => h.DrawsEvent("Sloppy Search"))
                .When.Player(p => p.SelectsEventOption("Roll", x => x.Rolls(1)))
                .Then.Player(p => p.Event(e => e.ActiveRow("Spend 1 Grace or lose 1 Secrecy").HasOptions("Lose Secrecy")));
        }
    }
}

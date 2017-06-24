using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class AltarTest
    {
        [TestCase(1, "Spend Secrecy", 1, 0)]
        [TestCase(1, "Continue", 0, 1)]
        [TestCase(0, "Continue", 0, 0)]
        public void PureAltar(int startingSecrecy, string option, int expectedGrace, int expectedSecrecy)
        {
            var expectedOptions = startingSecrecy > 0
                ? new[] {"Spend Secrecy", "Continue"}
                : new[] {"Continue"};
            const int roll = 4;
            TestScenario
                .Given.Game(g => g.Darkness(0).Hero("Acolyte", x => x.Secrecy(startingSecrecy).Grace(0)))
                .When.Hero(x => x.DrawsEvent("Altar"))
                .Then.Player(p => p.Event(e => e.HasBody("Altar", 3, "Roll 1d and take the highest").HasOptions("Roll")))
                .When.Player(p => p.SelectsEventOption("Roll", x => x.Rolls(roll)))
                .Then.Player(p => p.Event(e => e.ActiveRow("Pure Altar", "You may spend 1 Secrecy to gain 1 Grace")))
                .Then.Hero(h => h.Event(e => e.HasOutstanding(1).CanBeIgnored(false))
                    .Grace(0).Secrecy(startingSecrecy))
                .When.Player(p => p.AcceptsRoll())
                .Then.Player(p => p.Event(e => e.HasOptions(expectedOptions)))
                .When.Player(p => p.SelectsEventOption(option))
                .Then.Hero(h => h.Grace(expectedGrace).Secrecy(expectedSecrecy));
        }

        [TestCase(1, "Spend Grace", 0, 0)]
        [TestCase(1, "+1 Darkness", 1, 1)]
        [TestCase(0, "+1 Darkness", 0, 1)]
        public void DefiledAltar(int startingGrace, string option, int expectedGrace, int expectedDarkness)
        {
            var expectedOptions = startingGrace > 0
                ? new[] { "Spend Grace", "+1 Darkness" }
                : new[] { "+1 Darkness" };
            const int roll = 3;
            TestScenario
                .Given.Game(g => g.Darkness(0).Hero("Acolyte", x => x.Grace(startingGrace)))
                .When.Hero(x => x.DrawsEvent("Altar"))
                .Then.Player(p => p.Event(e => e.HasBody("Altar", 3, "Roll 1d and take the highest").HasOptions("Roll")))
                .When.Player(p => p.SelectsEventOption("Roll", x => x.Rolls(roll)))
                .Then.Player(p => p.Event(e => e.ActiveRow("Defiled Altar", "Spend 1 Grace or +1 Darkness")))
                .When.Player(p => p.AcceptsRoll())
                .Then.Player(p => p.Event(e => e.HasOptions(expectedOptions)))
                .When.Player(p => p.SelectsEventOption(option))
                .Then.Hero(h => h.Grace(expectedGrace))
                .Then.Game(g => g.Darkness(expectedDarkness));
        }
    }
}

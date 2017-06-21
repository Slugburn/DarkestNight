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
            new TestScenario()
                .GivenHero("Acolyte", x => x.Grace(0).Secrecy(startingSecrecy))
                .WhenHero(x => x.DrawsEvent("Altar"))
                .ThenPlayer(p => p.SeesEvent("Altar", "Roll 1d and take the highest", 3, "Roll"))
                .WhenPlayer(p => p.SelectsEventOption("Roll", x => x.Rolls(4)))
                .ThenPlayer(p=> p.SeesActiveEventRow(4,6, "Pure Altar"))
                .WhenPlayer(p => p.AcceptsRoll())
                .ThenPlayer(p => p.SeesEvent("Pure Altar", "You may spend 1 Secrecy to gain 1 Grace", 3, expectedOptions))
                .WhenPlayer(p => p.SelectsEventOption(option))
                .ThenHero(h => h.Grace(expectedGrace).Secrecy(expectedSecrecy));
        }

        [TestCase(1, "Spend Grace", 0, 0)]
        [TestCase(1, "+1 Darkness", 1, 1)]
        [TestCase(0, "+1 Darkness", 0, 1)]
        public void DefiledAltar(int startingGrace, string option, int expectedGrace, int expectedDarkness)
        {
            var expectedOptions = startingGrace > 0
                ? new[] { "Spend Grace", "+1 Darkness" }
                : new[] { "+1 Darkness" };
            new TestScenario()
                .GivenDarkness(0)
                .GivenHero("Acolyte", x => x.Grace(startingGrace))
                .WhenHero(x => x.DrawsEvent("Altar"))
                .ThenPlayer(p => p.SeesEvent("Altar", "Roll 1d and take the highest", 3, "Roll"))
                .WhenPlayer(p => p.SelectsEventOption("Roll", x => x.Rolls(3)).AcceptsRoll())
                .ThenPlayer(p => p.SeesEvent("Defiled Altar", "Spend 1 Grace or +1 Darkness", 3, expectedOptions))
                .WhenPlayer(p => p.SelectsEventOption(option))
                .ThenHero(h => h.Grace(expectedGrace))
                .ThenDarkness(expectedDarkness);
        }
    }
}

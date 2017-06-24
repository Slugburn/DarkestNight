using NUnit.Framework;
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
                .Given.Game.Darkness(0).WithHero("Acolyte").Secrecy(startingSecrecy).Grace(0)
                .When.Hero.DrawsEvent("Altar")
                .Then().Player.EventView.HasBody("Altar", 3, "Roll 1d and take the highest").HasOptions("Roll")
                .When.Player.SelectsEventOption("Roll", Fake.Rolls(roll))
                .Then().Player.EventView.ActiveRow("Pure Altar", "You may spend 1 Secrecy to gain 1 Grace")
                .Then(Verify.Hero.HasUnresolvedEvents(1).Grace(0).Secrecy(startingSecrecy))
                .Then().Player.EventView.HasOptions(expectedOptions)
                .When.Player.SelectsEventOption(option)
                .Then(Verify.Hero.Grace(expectedGrace).Secrecy(expectedSecrecy));
        }

        [TestCase(1, "Spend Grace", 0, 0)]
        [TestCase(1, "+1 Darkness", 1, 1)]
        [TestCase(0, "+1 Darkness", 0, 1)]
        public void DefiledAltar(int startingGrace, string option, int expectedGrace, int expectedDarkness)
        {
            var expectedOptions = startingGrace > 0
                ? new[] {"Spend Grace", "+1 Darkness"}
                : new[] {"+1 Darkness"};
            const int roll = 3;
            TestScenario
                .Given.Game.Darkness(0).WithHero("Acolyte").Grace(startingGrace)
                .When.Hero.DrawsEvent("Altar")
                .Then().Player.EventView.HasBody("Altar", 3, "Roll 1d and take the highest").HasOptions("Roll")
                .When.Player.SelectsEventOption("Roll", Fake.Rolls(roll))
                .Then().Player.EventView.ActiveRow("Defiled Altar", "Spend 1 Grace or +1 Darkness").HasOptions(expectedOptions)
                .When.Player.SelectsEventOption(option)
                .Then(Verify.Hero.Grace(expectedGrace))
                .Then(Verify.Game.Darkness(expectedDarkness));
        }
    }
}
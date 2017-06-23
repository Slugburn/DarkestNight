﻿using NUnit.Framework;
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
            new TestScenario()
                .GivenDarkness(0)
                .GivenHero("Acolyte", x => x.Secrecy(startingSecrecy).Grace(0))
                .WhenHero(x => x.DrawsEvent("Altar"))
                .ThenPlayer(p => p.Event(e => e.HasBody("Altar", 3, "Roll 1d and take the highest").HasOptions("Roll")))
                .WhenPlayer(p => p.SelectsEventOption("Roll", x => x.Rolls(roll)))
                .ThenPlayer(p => p.Event(e => e.ActiveRow(4, 6, "Pure Altar", "You may spend 1 Secrecy to gain 1 Grace")))
                .WhenPlayer(p => p.AcceptsRoll())
                .ThenPlayer(p => p.Event(e => e.HasOptions(expectedOptions)))
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
            const int roll = 3;
            new TestScenario()
                .GivenDarkness(0)
                .GivenHero("Acolyte", x => x.Grace(startingGrace))
                .WhenHero(x => x.DrawsEvent("Altar"))
                .ThenPlayer(p => p.Event(e => e.HasBody("Altar", 3, "Roll 1d and take the highest").HasOptions("Roll")))
                .WhenPlayer(p => p.SelectsEventOption("Roll", x => x.Rolls(roll)))
                .ThenPlayer(p => p.Event(e => e.ActiveRow(1, 3, "Defiled Altar", "Spend 1 Grace or +1 Darkness")))
                .WhenPlayer(p => p.AcceptsRoll())
                .ThenPlayer(p => p.Event(e => e.HasOptions(expectedOptions)))
                .WhenPlayer(p => p.SelectsEventOption(option))
                .ThenHero(h => h.Grace(expectedGrace))
                .ThenDarkness(expectedDarkness);
        }
    }
}
﻿using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Actions
{
    [TestFixture]
    public class TravelTest
    {
        [Test]
        public void Travel()
        {
            TestScenario.Game
                .WithHero().At("Monastery").Secrecy(4)
                .When.Player.TakesAction("Travel")
                .Then(Verify.Player.LocationSelectionView("Mountains", "Village", "Forest"))
                .When.Player.SelectsDestination("Forest")
                .Then(Verify.Hero().HasUsedAction().Location("Forest").Secrecy(5));
        }
        [Test]

        [TestCase(5)]
        [TestCase(6)]
        public void Travel_MaxFiveSecrecy(int secrecy)
        {
            TestScenario.Game
                .WithHero().At("Monastery").Secrecy(secrecy)
                .When.Player.TakesAction("Travel")
                .Then(Verify.Player.LocationSelectionView("Mountains", "Village", "Forest"))
                .When.Player.SelectsDestination("Forest")
                .Then(Verify.Hero().HasUsedAction().Location("Forest").Secrecy(secrecy));
        }

        [Test]
        public void Travel_MoveTwo()
        {
            // should only gain 1 secrecy even though moving two spaces
            TestScenario.Game
                .WithHero("Druid").At("Monastery").HasPowers("Raven Form").Power("Raven Form").IsActive().Secrecy(0)
                .When.Player.TakesAction("Travel")
                .Then(Verify.Player.LocationSelectionView("Mountains", "Village", "Forest"))
                .When.Player.SelectsDestination("Mountains")
                .Then(Verify.Player.LocationSelectionView("Monastery", "Village", "Castle"))
                .When.Player.SelectsDestination("Castle")
                .Then(Verify.Hero().HasUsedAction().Location("Castle").CanGainGrace(false).Secrecy(1));
        }

        [Test]
        public void Travel_CanMoveTwoButOnlyWantToMoveOne()
        {
            TestScenario.Game
                .WithHero("Druid").At("Monastery").HasPowers("Raven Form").Power("Raven Form").IsActive().Secrecy(0)
                .When.Player.TakesAction("Travel")
                .Then(Verify.Player.LocationSelectionView("Mountains", "Village", "Forest"))
                .When.Player.SelectsDestination("Mountains")
                .Then(Verify.Player.LocationSelectionView("Monastery", "Village", "Castle"))
                .Then(Verify.Player.Hero("Druid").Commands.Exactly("Done Moving"))
                .When.Player.TakesAction("Done Moving")
                .Then(Verify.Hero().HasUsedAction().Location("Mountains").CanGainGrace(false).Secrecy(1));
        }
    }
}

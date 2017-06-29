using NUnit.Framework;
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
                .When.Player.SelectsLocation("Forest")
                .Then(Verify.Hero().Location("Forest").Secrecy(5));
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
                .When.Player.SelectsLocation("Forest")
                .Then(Verify.Hero().Location("Forest").Secrecy(secrecy));
        }

        [Test]
        public void Travel_MoveTwo()
        {
            TestScenario.Game
                .WithHero("Druid").At("Monastery").HasPowers("Raven Form").Power("Raven Form").IsActive().Secrecy(0)
                .When.Player.TakesAction("Travel")
                .Then(Verify.Player.LocationSelectionView("Mountains", "Village", "Forest"))
                .When.Player.SelectsLocation("Mountains")
                .Then(Verify.Player.LocationSelectionView("Monastery", "Village", "Castle"))
                .When.Player.SelectsLocation("Castle")
                .Then(Verify.Hero().Location("Castle").CanGainGrace(false).Secrecy(1));
        }
    }
}

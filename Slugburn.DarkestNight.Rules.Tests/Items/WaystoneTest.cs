using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Items
{
    [TestFixture]
    public class WaystoneTest 
    {
        [Test]
        // Discard during your turn to instantly move to any location and gain 1 Secrecy.
        public void Waystone()
        {
            TestScenario.Game
                .WithHero().HasItems("Waystone").At("Castle").Secrecy(0)
                .When.Player.TakesAction("Waystone")
                .Then(Verify.Player.LocationSelectionView("Monastery", "Mountains", "Village", "Ruins", "Swamp", "Forest"))
                .When.Player.SelectsLocation("Monastery")
                .Then(Verify.Hero().HasNotUsedAction().Location("Monastery").Secrecy(1).HasItems());
        }
    }
}

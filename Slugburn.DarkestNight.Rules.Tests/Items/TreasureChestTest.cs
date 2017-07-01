using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Items
{
    [TestFixture]
    public class TreasureChestTest
    {
        [Test]
        public void UseIt()
        {
            // Discard at any time to draw a new power card.
            TestScenario.Game
                .WithHero("Druid").HasItems("Treasure Chest").NextPowerDraws("Wolf Form")
                .When.Player.TakesAction("Treasure Chest")
                .Then(Verify.Hero().HasPowers("Wolf Form").HasItems());
        }
    }
}

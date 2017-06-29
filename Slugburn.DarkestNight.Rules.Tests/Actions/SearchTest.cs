using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Actions
{
    [TestFixture]
    public class SearchTest
    {
        [TestCase(Find.Key, "Key")]
        [TestCase(Find.TreasureChest, "Treasure Chest")]
        [TestCase(Find.BottledMagic, "Bottled Magic")]
        [TestCase(Find.VanishingDust, "Vanishing Dust")]
        [TestCase(Find.Waystone, "Waystone")]
        public void FindItem(Find find, string itemName)
        {
            TestScenario
                .Game.WithHero().At("Village")
                .NextSearchResult(find)
                .When.Player.TakesAction("Search").AcceptsRoll()
                .Then(Verify.Hero().HasUsedAction().HasItems(itemName));
        }

        [Test]
        public void FindSupplyCache()
        {
            // Draw two power cards, keep one, place the other at the bottom of your deck
            TestScenario
                .Game.WithHero("Acolyte").At("Village").HasPowers("Final Rest")
                .NextSearchResult(Find.SupplyCache)
                .Given.Hero().NextPowerDraws("Call to Death", "False Life")
                .When.Player.TakesAction("Search").AcceptsRoll()
                .Then(Verify.Player.PowerSelectionView("Call to Death", "False Life"))
                .When.Player.SelectsPower("Call to Death")
                .Then(Verify.Hero().HasUsedAction().HasPowers("Call to Death", "Final Rest").PowerDeckContains("False Life"));
        }

        [Test]
        public void ForgottenShrine()
        {
            // Gain 2 Grace (no max)
            TestScenario
                .Game.WithHero().NotAt("Monastery").Grace(5)
                .NextSearchResult(Find.ForgottenShrine)
                .When.Player.TakesAction("Search").AcceptsRoll()
                .Then(Verify.Hero().HasUsedAction().Grace(7));
        }

        [Test]
        public void Epiphany()
        {
            // Search your power deck and take the card of your choice, then shuffle that deck
        }
    }
}
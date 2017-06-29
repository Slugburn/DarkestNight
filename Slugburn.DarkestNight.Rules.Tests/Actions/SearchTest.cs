using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Actions
{
    [TestFixture]
    public class SearchTest
    {
        [Test]
        public void MultipleSearchChoices()
        {
            TestScenario.Game
                .WithHero("Druid").NotAt("Monastery").HasPowers("Raven Form").Power("Raven Form").IsActive()
                .NextSearchResult(Find.Key, Find.TreasureChest)
                .When.Player.TakesAction("Search", Fake.Rolls(5, 6))
                .Then(Verify.Player.SearchResultSelection.Roll(5, 6))
                .When.Player.AcceptsRoll()
                .Then(Verify.Player.SearchResultSelection.Results("Key", "Treasure Chest").Roll(5, 6))
                .When.Player.SelectsSearchResult("Treasure Chest")
                .Then(Verify.Hero().HasItems("Treasure Chest").HasUsedAction().CanGainGrace(false));
        }

        [TestCase(Find.Key, "Key")]
        [TestCase(Find.TreasureChest, "Treasure Chest")]
        [TestCase(Find.BottledMagic, "Bottled Magic")]
        [TestCase(Find.VanishingDust, "Vanishing Dust")]
        [TestCase(Find.Waystone, "Waystone")]
        public void FindItem(Find find, string itemName)
        {
            TestScenario.Game
                .WithHero().At("Village")
                .NextSearchResult(find)
                .When.Player.CompletesSearch()
                .Then(Verify.Hero().HasUsedAction().HasItems(itemName));
        }

        [Test]
        public void FindSupplyCache()
        {
            // Draw two power cards, keep one, place the other at the bottom of your deck
            TestScenario.Game
                .WithHero("Acolyte").At("Village").HasPowers("Final Rest")
                .NextSearchResult(Find.SupplyCache)
                .Given.Hero().NextPowerDraws("Call to Death", "False Life")
                .When.Player.CompletesSearch()
                .Then(Verify.Player.PowerSelectionView("Call to Death", "False Life"))
                .When.Player.SelectsPower("Call to Death")
                .Then(Verify.Hero().HasUsedAction()
                    .HasPowers("Call to Death", "Final Rest")
                    .PowerDeckContains("Blinding Black", "Dark Veil", "Death Mask", "Fade to Black", "False Orders",
                        "Forbidden Arts", "Leech Life", "False Life"));
        }

        [Test]
        public void ForgottenShrine()
        {
            // Gain 2 Grace (no max)
            TestScenario.Game
                .WithHero().NotAt("Monastery").Grace(5)
                .NextSearchResult(Find.ForgottenShrine)
                .When.Player.CompletesSearch()
                .Then(Verify.Hero().HasUsedAction().Grace(7));
        }

        [Test]
        public void Epiphany()
        {
            // Search your power deck and take the card of your choice, then shuffle that deck
            TestScenario.Game
                .WithHero("Druid").NotAt("Monastery").HasPowers("Animal Companion", "Sprite Form", "Visions")
                .NextSearchResult(Find.Epiphany)
                .When.Player.CompletesSearch()
                .Then(Verify.Player.PowerSelectionView("Camouflage", "Celerity", "Raven Form", "Tranquility", "Tree Form", "Vines", "Wolf Form"))
                .When.Player.SelectsPower("Raven Form")
                .Then(Verify.Hero().HasUsedAction()
                    .HasPowers("Animal Companion", "Sprite Form", "Visions", "Raven Form")
                    .PowerDeckContains("Camouflage", "Celerity", "Tranquility", "Tree Form", "Vines", "Wolf Form"));
        }

        [Test]
        public void Artifact()
        {
            // Draw an artifact card
            TestScenario.Game
                .WithHero().NotAt("Monastery")
                .NextSearchResult(Find.Artifact)
                .NextArtifact("Ghost Mail")
                .When.Player.CompletesSearch()
                .Then(Verify.Hero().HasUsedAction().HasItems("Ghost Mail"));

        }
    }
}
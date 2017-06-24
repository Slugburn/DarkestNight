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
                .Given.Game.Hero("Acolyte", x => x.Location("Village"))
                .NextSearchResult(find)
                .When.Player.TakesAction("Search")
                .When.Player.AcceptsRoll()
                .Then.Hero(x => x.HasUsedAction().HasItem(itemName));
        }
    }
}
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
            new TestScenario()
                .GivenHero("Acolyte", x => x.Location("Village"))
                .GivenNextSearchResult(find)
                .WhenPlayer(x => x.TakesAction("Search"))
                .WhenPlayer(x=>x.AcceptsRoll())
                .ThenHero(x => x.HasUsedAction().HasItem(itemName));
        }
    }
}

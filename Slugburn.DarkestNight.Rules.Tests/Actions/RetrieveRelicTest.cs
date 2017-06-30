using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Actions
{
    [TestFixture]
    public class RetrieveRelicTest
    {
        // Need to have 3 keys and be at a location with a relic in order to use this action
        [Test]
        public void HaveThreeKeysAndAtLocationWithRelic()
        {
            TestScenario.Game
                .Location("Mountains").HasRelic()
                .WithHero().At("Mountains").HasItems("Key", "Key", "Key")
                .When.Player.TakesAction(RetrieveRelic.ActionName)
                .Then(Verify.Hero().HasUsedAction().HasItems("Holy Relic"))
                .Then(Verify.Location("Mountains").HasRelic(false).DoesNotHaveAction(RetrieveRelic.ActionName));
        }

        [Test]
        public void HeroHasMoreThanThreeKeys()
        {
            TestScenario.Game
                .Location("Mountains").HasRelic()
                .WithHero().At("Mountains").HasItems("Key", "Key", "Key", "Key")
                .When.Player.TakesAction(RetrieveRelic.ActionName)
                .Then(Verify.Hero().HasUsedAction().HasItems("Holy Relic", "Key"));
        }

        [Test]
        public void KeysSplitBetweenTwoHeroes()
        {
            TestScenario.Game
                .Location("Mountains").HasRelic()
                .WithHero("Druid").At("Mountains").HasItems("Key", "Key")
                .WithHero("Priest").At("Mountains").HasItems("Key", "Key")
                .Given.Hero("Druid").IsTakingTurn()
                .When.Player.TakesAction("Druid", RetrieveRelic.ActionName)
                .Then(Verify.Hero("Druid").HasUsedAction().HasItems("Holy Relic"))
                .Then(Verify.Hero("Priest").HasItems("Key"));
        }

        [Test]
        public void NotEnoughKeys()
        {
            TestScenario.Game
                .Location("Mountains").HasRelic()
                .WithHero().At("Mountains").HasItems("Key", "Key")
                .Then(Verify.Hero().CanTakeAction(RetrieveRelic.ActionName, false));
        }

        [Test]
        public void LocationDoesNotHaveRelic()
        {
            TestScenario.Game
                .Location("Mountains").HasRelic(false)
                .WithHero().At("Mountains").HasItems("Key", "Key", "Key")
                .Then(Verify.Hero().CanTakeAction(RetrieveRelic.ActionName, false));
        }

        
    }
}

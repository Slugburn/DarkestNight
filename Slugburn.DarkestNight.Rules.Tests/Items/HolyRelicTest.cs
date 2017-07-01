using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Items
{
    [TestFixture]
    public class HolyRelicTest
    {
        // Each hero can only carry one at a time.
        [Test]
        public void CanOnlyCarryOne_CanNotRetrieveANewRelic()
        {
            // Each hero can only carry one at a time.
            TestScenario.Game
                .WithHero().HasItems("Holy Relic", "Key", "Key", "Key").At("Mountains")
                .Location("Mountains").HasRelic()
                .Then(Verify.Hero().CanTakeAction(RetrieveRelic.ActionName, false));
        }

        [Test]
        public void CanOnlyCarryOne_TradeIsRejected()
        {
            TestScenario.Game
                .WithHero("Knight").At("Village").HasItems("Holy Relic")
                .WithHero("Druid").At("Village").HasItems("Holy Relic")
                .When.Player.TradesItem("Holy Relic", "Knight", "Druid")
                .Then(Verify.Hero("Druid").HasItems("Holy Relic"));
        }

        [Test]
        public void AddOneToHighestFightDie()
        {
            // Holy relics add 1 to your highest die in fights.
            TestScenario.Game
                .WithHero("Knight").HasItems("Holy Relic").HasPowers("Reckless Abandon").IsFacingEnemy("Zombie")
                .When.Player.CompletesConflict("Zombie", "Reckless Abandon", Fake.Rolls(3, 6, 1, 4))
                .Then(Verify.Hero().Rolled(3, 7, 1, 4));
        }

        [Test]
        public void LoseSecrecyAtStartOfTurn()
        {
            // You lose 1 Secrecy at the start of your turn while carrying a holy relic.
            TestScenario.Game
                .WithHero().HasItems("Holy Relic").IsTakingTurn(false)
                .When.Player.TakesAction("Start Turn")
                .Then(Verify.Hero().LostSecrecy());
        }

        [Test]
        public void LoseSecrecyWhenReceivedFromAnotherHero()
        {
            // You lose 1 Secrecy if another hero gives you a holy relic.
            TestScenario.Game
                .WithHero("Knight").At("Village").HasItems("Holy Relic")
                .WithHero("Druid").At("Village")
                .When.Player.TradesItem("Holy Relic", "Knight", "Druid")
                .Then(Verify.Hero("Druid").LostSecrecy());
        }
    }
}

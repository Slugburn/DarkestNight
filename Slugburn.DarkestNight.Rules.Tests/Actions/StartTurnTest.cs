using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Actions
{
    [TestFixture]
    public class StartTurnTest
    {
        // Action is available when:
        // * It's not the necromancer's turn
        // * No hero is currently taking their turn
        // * The hero hasn't already taken their turn
        [Test]
        public void ActionAvailable()
        {
            TestScenario.Game
                .Necromancer.IsTakingTurn(false)
                .WithHero("Knight").IsTakingTurn(false)
                .WithHero("Priest").IsTakingTurn(false).HasTakenTurn(false)
                .Then(Verify.Hero("Priest").CanTakeAction("Start Turn"));
        }

        [Test]
        public void ActionNotAvailable_NecromancerIsActing()
        {
            TestScenario.Game
                .Necromancer.IsTakingTurn()
                .WithHero()
                .Then(Verify.Hero().CanTakeAction("Start Turn", false));
        }

        [Test]
        public void ActionNotAvailable_AnotherHeroIsTakingTurn()
        {
            TestScenario.Game
                .WithHero("Knight").WithHero("Priest")
                .Given.Hero("Knight").IsTakingTurn()
                .Then(Verify.Hero("Priest").CanTakeAction("Start Turn", false));
        }

        [Test]
        public void ActionNotAvailable_AlreadyTookTurn()
        {
            TestScenario.Game
                .WithHero().HasTakenTurn()
                .Then(Verify.Hero().CanTakeAction("Start Turn", false));
        }

        [Test]
        public void LoseSecrecyIfInSameLocationAsNecromancer()
        {
            TestScenario.Game
                .Necromancer.At("Village")
                .WithHero().At("Village").IsTakingTurn(false)
                .When.Player.TakesAction("Start Turn")
                .Then(Verify.Hero().LostSecrecy().HasUnresolvedEvents(1));
        }

        [Test]
        public void LoseSecrecyIfCarryingHolyRelic()
        {
            TestScenario.Game
                .WithHero().HasItems("Holy Relic").IsTakingTurn(false)
                .When.Player.TakesAction("Start Turn")
                .Then(Verify.Hero().LostSecrecy());
        }

        [Test]
        public void FightNecromancerWhenNoSecrecy()
        {
            // fight necromancer
            TestScenario.Game
                .Necromancer.At("Castle")
                .WithHero().Secrecy(0).At("Castle").IsTakingTurn(false)
                .When.Player.TakesAction("Start Turn")
                .Then(Verify.Hero().Secrecy(0).IsFacingEnemies("Necromancer").HasUnresolvedEvents(0))
                .Then(Verify.Player.ConflictModel.HasTargets("Necromancer"));
        }

        [Test]
        public void NoEventDrawnAtMonastery()
        {
            TestScenario.Game
                .WithHero().At("Monastery").IsTakingTurn(false)
                .When.Player.TakesAction("Start Turn")
                .Then(Verify.Hero().HasUnresolvedEvents(0))
                .Then(Verify.Player.EventView.IsVisible(false));
        }


        [Test]
        public void NoSecrecyButNotWithNecromancer()
        {
            // draw event 
            TestScenario.Game
                .WithHero().Secrecy(0).NotAt("Monastery").IsTakingTurn(false)
                .When.Player.TakesAction("Start Turn")
                .Then(Verify.Hero().Secrecy(0).HasUnresolvedEvents(1))
                .Then(Verify.Player.EventView.IsVisible());
        }

        [Test]
        public void WithNecromancerButHaveSecrecy()
        {
            // draw event instead of fighting necromancer            
            TestScenario.Game
                .Necromancer.At("Castle")
                .WithHero().At("Castle").IsTakingTurn(false)
                .When.Player.TakesAction("Start Turn")
                // lose secrecy because of Necromancer, but draw event instead of fighting
                .Then(Verify.Hero().LostSecrecy().HasUnresolvedEvents(1).IsFacingEnemies()) 
                .Then(Verify.Player.EventView.IsVisible());
        }

    }
}

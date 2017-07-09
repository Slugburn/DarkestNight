using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Actions
{
    [TestFixture]
    public class EndTurnTest
    {
        [Test]
        public void GainSecrecyIfSpentEntireTurnInMonastery()
        {
            TestScenario.Game
                .WithHero().Secrecy(0).At("Monastery")
                .When.Player.TakesAction("Hide")
                .Then(Verify.Hero().HasUsedAction().Secrecy(2));
        }

        [Test]
        public void DoNotGainSecrecyIfEntireTurnNotSpentInMonastery()
        {
            TestScenario.Game
                .WithHero().Secrecy(0).At("Monastery").HasItems("Waystone")
                .When.Player.TakesAction("Waystone").SelectsLocation("Village")
                .When.Player.TakesAction("Travel").SelectsLocation("Monastery")
                .Then(Verify.Hero().HasUsedAction()
                    .Secrecy(2)); // gained 1 from using Waystone and 1 from Travel
        }

        [Test]
        public void AfterAllPlayersHaveFinishedTheirTurn()
        {
            TestScenario.Game
                .WithHero("Acolyte").At("Monastery").IsTakingTurn(false)
                .WithHero("Priest").At("Monastery").IsTakingTurn(false)
                .When.Player.TakesAction("Acolyte", "Start Turn").TakesAction("Acolyte", "End Turn")
                .When.Player.TakesAction("Priest", "Start Turn").TakesAction("Priest", "End Turn")
                .Then(Verify.Game.Darkness(1).Necromancer.IsTakingTurn());
        }
    }
}

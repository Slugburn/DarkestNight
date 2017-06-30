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
                .When.Player.TakesAction("End Turn")
                .Then(Verify.Hero().Secrecy(1));
        }

        [Test]
        public void DoNotGainSecrecyIfEntireTurnNotSpendInMonastery()
        {
            TestScenario.Game
                .WithHero().Secrecy(0).At("Monastery")
                .When.Player.TakesAction("Waystone").SelectsLocation("Village")
                .When.Player.TakesAction("Travel").SelectsLocation("Monastery")
                .When.Player.TakesAction("End Turn")
                .Then(Verify.Hero().Secrecy(0).HasUsedAction());
        }
    }
}

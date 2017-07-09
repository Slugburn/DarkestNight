using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Actions
{
    [TestFixture]
    public class HideTest
    {
        [Test]
        public void BelowFiveSecrecy()
        {
            TestScenario.Game
                .WithHero().Secrecy(4)
                .When.Player.TakesAction("Hide")
                .Then(Verify.Hero().HasUsedAction().Secrecy(5));
        }

        [Test]
        public void PowersExhausted()
        {
            TestScenario.Game
                .WithHero("Priest").Secrecy(5).HasPowers("Calm", "Sanctuary")
                .Power("Calm").IsExhausted()
                .Power("Sanctuary").IsExhausted()
                .When.Player.TakesAction("Hide")
                .Then(Verify.Hero().HasUsedAction())
                .Then(Verify.Power("Calm").IsExhausted(false))
                .Then(Verify.Power("Sanctuary").IsExhausted(false));
        }
    }
}

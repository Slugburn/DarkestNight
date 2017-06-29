using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Actions
{
    [TestFixture]
    public class PrayTest
    {
        // Roll 2d, and gain 1 Grace (up to default) for each die that rolls a 3 or higher. Also refresh your powers.
        [Test]
        public void FullSuccess()
        {
            TestScenario.Game
                .WithHero("Priest").Grace(0).At("Monastery").HasPowers("Censure").Power("Censure").IsExhausted()
                .When.Player.TakesAction("Pray", Fake.Rolls(3,3))
                .Then(Verify.Player.PrayerView.Roll(3,3).Before(0).After(2))
                .When.Player.AcceptsRoll()
                .Then(Verify.Hero().HasUsedAction().Grace(2))
                .Then(Verify.Power("Censure").IsExhausted(false));
        }

        [Test]
        public void WontIncreaseGraceAboveDefault()
        {
            TestScenario.Game
                .WithHero().DefaultGrace(5).Grace(4).At("Monastery")
                .When.Player.TakesAction("Pray", Fake.Rolls(3, 3))
                .Then(Verify.Player.PrayerView.Roll(3, 3).Before(4).After(5))
                .When.Player.AcceptsRoll()
                .Then(Verify.Hero().HasUsedAction().Grace(5));
        }

        [TestCase(6)]
        [TestCase(7)]
        public void NotAvailableIfAtOrAboveDefaultGraceAndNoPowersExhausted(int grace)
        {
            TestScenario.Game
                .WithHero("Priest").DefaultGrace(6).Grace(grace).At("Monastery").HasPowers("Censure")
                .Then(Verify.Hero().Grace(grace).CanTakeAction("Pray", false));
        }
    }
}

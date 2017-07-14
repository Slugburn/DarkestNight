using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class PrinceTest
    {
        // Chapel (Action): Spend 1 Secrecy to activate in your location. 
        // Active: Heroes may pray there.
        [Test]
        public void Chapel_Activate()
        {
            TestScenario.Game
                .WithHero("Prince").HasPowers("Chapel").At("Village")
                .When.Player.TakesAction("Chapel")
                .Then(Verify.Location("Village").HasAction("Pray [Chapel]"))
                .Then(Verify.Power("Chapel").IsActive())
                .Then(Verify.Player.Hero("Prince").LostSecrecy(1));
        }

        [Test]
        public void Chapel_UseAction()
        {
            TestScenario.Game
                .WithHero("Knight").Grace(0).At("Village")
                .WithHero("Prince").At("Village").HasPowers("Chapel")
                .When.Player.TakesAction("Chapel")
                .Given.Hero("Knight").IsTakingTurn()
                .Then(Verify.Player.Hero("Knight").Commands.Includes("Pray [Chapel]"))
                .When.Player.TakesAction("Knight", "Pray [Chapel]", Fake.Rolls(3, 3))
                .Then(Verify.Player.PrayerView.Roll(3, 3).Before(0).After(2))
                .When.Player.AcceptsRoll()
                .Then(Verify.Player.Hero("Knight").Grace(2));
        }

    }
}
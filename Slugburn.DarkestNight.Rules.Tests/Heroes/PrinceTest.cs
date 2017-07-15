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
                .WithHero("Prince").HasPowers("Chapel").Power("Chapel").IsActive("Village")
                .WithHero("Knight").Grace(0).At("Village")
                .Given.Hero("Knight").IsTakingTurn()
                .Then(Verify.Player.Hero("Knight").Commands.Includes("Pray [Chapel]"))
                .When.Player.TakesAction("Knight", "Pray [Chapel]", Fake.Rolls(3, 3))
                .Then(Verify.Player.PrayerView.Roll(3, 3).Before(0).After(2))
                .When.Player.AcceptsRoll()
                .Then(Verify.Player.Hero("Knight").Grace(2));
        }

        // Divine Right (Bonus): +1 to default Grace. Add 1 to each die when praying.
        [Test]
        public void DivineRight()
        {
            TestScenario.Game
                .WithHero("Prince").HasPowers("Divine Right").Grace(0)
                .Then(Verify.Hero().DefaultGrace(5).Grace(0))
                .When.Player.TakesAction("Pray", Fake.Rolls(2, 3))
                .Then(Verify.Player.PrayerView.Roll(3, 4).Before(0).After(2))
                .When.Player.AcceptsRoll()
                .Then(Verify.Player.Hero("Prince").Grace(2).DefaultGrace(5));
        }

        // Inspire (Action): Activate on a hero in your location. Deactivate before any die roll for +3d.
        [Test]
        public void Inspire()
        {
            TestScenario.Game
                .WithHero("Prince").At("Mountains").HasPowers("Inspire")
                .WithHero("Knight").At("Mountains")
                .Given.Hero("Prince").IsTakingTurn()
                .When.Player.TakesAction("Inspire").SelectsHero("Knight")
                .Given.Hero("Knight").IsTakingTurn()
                .When.Player.TakesAction("Search", Fake.Rolls(1))
                .When.Player.TakesAction("Prince", "Deactivate Inspire", Fake.Rolls(2, 4, 6))
                .Then(Verify.Player.SearchView.Roll(1, 2, 4, 6));
        }
    }
}
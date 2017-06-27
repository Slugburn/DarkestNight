using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class TwistOfFateTest
    {
        [TestCase(6)]
        [TestCase(5)]
        public void TwistOfFate_Bonus(int roll)
        {
            TestScenario
                .Game.WithHero()
                .Given.Hero().HasDrawnEvent("Twist of Fate")
                .Then(Verify.Player.EventView.HasBody("Twist of Fate", 1, "Roll 1d and take the highest").HasOptions("Roll"))
                .When.Player.SelectsEventOption("Roll", Fake.Rolls(roll))
                .Then(Verify.Player.EventView.ActiveRow("+1d on all rolls for the rest of this turn"))
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Hero.HasUnresolvedEvents(0).HasDieModifier("Twist of Fate", RollType.Any, 1).FightDice(2).EludeDice(2).SearchDice(2))
                .When.Player.TakesAction("End Turn")
                .Then(Verify.Hero.HasNoDieModifier());
        }

        [TestCase(4)]
        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public void TwistOfFate_Penalty(int roll)
        {
            TestScenario
                .Game.WithHero()
                .Given.Hero().HasDrawnEvent("Twist of Fate")
                .Then(Verify.Player.EventView.HasBody("Twist of Fate", 1, "Roll 1d and take the highest").HasOptions("Roll"))
                .When.Player.SelectsEventOption("Roll", Fake.Rolls(roll))
                .Then(Verify.Player.EventView.ActiveRow("-1d (to a minimum of 1d) on all rolls for the rest of this turn"))
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Hero.HasUnresolvedEvents(0).HasDieModifier("Twist of Fate", RollType.Any, -1))
                .When.Player.TakesAction("End Turn")
                .Then(Verify.Hero.HasNoDieModifier());
        }
    }
}

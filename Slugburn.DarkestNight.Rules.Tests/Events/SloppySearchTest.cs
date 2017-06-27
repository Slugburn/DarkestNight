using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class SloppySearchTest
    {
        [TestCase(5)]
        [TestCase(4)]
        public void SloppySearch_NoEffect(int roll)
        {
            TestScenario
                .Game.WithHero()
                .Given.Hero().HasDrawnEvent("Sloppy Search")
                .When.Player.SelectsEventOption("Roll", Fake.Rolls(roll))
                .Then(Verify.Player.EventView.ActiveRow("No effect").HasOptions("No Effect"))
                .When.Player.SelectsEventOption("No Effect")
                .Then(Verify.Hero().HasUnresolvedEvents(0));
        }

        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public void SloppySearch_SpendGrace(int roll)
        {
            TestScenario
                .Game.WithHero()
                .Given.Hero().HasDrawnEvent("Sloppy Search")
                .When.Player.SelectsEventOption("Roll", Fake.Rolls(roll))
                .Then(Verify.Player.EventView.ActiveRow("Spend 1 Grace or lose 1 Secrecy").HasOptions("Spend Grace", "Lose Secrecy"))
                .When.Player.SelectsEventOption("Spend Grace")
                .Then(Verify.Hero().LostGrace().HasUnresolvedEvents(0));
        }

        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public void SloppySearch_LoseSecrecy(int roll)
        {
            TestScenario
                .Game.WithHero()
                .Given.Hero().HasDrawnEvent("Sloppy Search")
                .When.Player.SelectsEventOption("Roll", Fake.Rolls(roll))
                .When.Player.SelectsEventOption("Lose Secrecy")
                .Then(Verify.Hero().LostSecrecy().HasUnresolvedEvents(0));
        }

        [Test]
        public void SloppySearch_GainSecrecy()
        {
            TestScenario
                .Game.WithHero().Secrecy(0)
                .Given.Hero().HasDrawnEvent("Sloppy Search")
                .Then(Verify.Player.EventView.HasBody("Sloppy Search", 2, "Roll 1d and take the highest").HasOptions("Roll"))
                .When.Player.SelectsEventOption("Roll", Fake.Rolls(6))
                .Then(Verify.Player.EventView.ActiveRow("Gain 1 Secrecy").HasOptions("Gain Secrecy"))
                .When.Player.SelectsEventOption("Gain Secrecy")
                .Then(Verify.Hero().Secrecy(1).HasUnresolvedEvents(0));
        }

        [Test]
        public void SloppySearch_NoGraceToSpend()
        {
            TestScenario
                .Game.WithHero().Grace(0)
                .Given.Hero().HasDrawnEvent("Sloppy Search")
                .When.Player.SelectsEventOption("Roll", Fake.Rolls(1))
                .Then(Verify.Player.EventView.ActiveRow("Spend 1 Grace or lose 1 Secrecy").HasOptions("Lose Secrecy"));
        }
    }
}
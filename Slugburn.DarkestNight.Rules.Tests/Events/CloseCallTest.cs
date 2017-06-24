using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class CloseCallTest
    {
        [TestCase(6, "No effect", 0, 0)]
        [TestCase(5, "No effect", 0, 0)]
        [TestCase(4, "Lose 1 Secrecy", 1, 0)]
        [TestCase(3, "Lose 1 Secrecy", 1, 0)]
        [TestCase(2, "Lose 1 Grace", 0, 1)]
        [TestCase(1, "Lose 1 Grace", 0, 1)]
        public void CloseCall(int roll, string effect, int lostSecrecy, int lostGrace)
        {
            TestScenario
                .Given.Game.WithHero()
                .When.Hero.DrawsEvent("Close Call")
                .Then(Verify.Player.EventView.HasBody("Close Call", 4, "Roll 1d and take the highest").HasOptions("Roll"))
                .When.Player.SelectsEventOption("Roll", Fake.Rolls(roll))
                .Then(Verify.Player.EventView.ActiveRow(effect))
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Hero.LostSecrecy(lostSecrecy).LostGrace(lostGrace));
        }
    }
}
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class UnfriendlyEyesTest
    {
        [Test]
        public void LoseSecrecy()
        {
            TestScenario
                .Given.Game.WithHero(x => x.At("Village"))
                .Given.Location("Village").Blight()
                .When.Hero.DrawsEvent("Unfriendly Eyes")
                .Then().Player.Event.HasBody("Unfriendly Eyes", 5, "Count the blights in your location").ActiveRow("Lose 1 Secrecy").HasOptions("Lose Secrecy")
                .When.Player.SelectsEventOption("Lose Secrecy")
                .Then(Verify.Hero.LostSecrecy());
        }
        [TestCase(1)]
        [TestCase(2)]
        public void SpendSecrecy(int blightCount)
        {
            var blights = Enumerable.Repeat("Lich", blightCount).ToArray();
            TestScenario
                .Given.Game.WithHero(x => x.At("Village"))
                .Given.Location("Village").Blight(blights)
                .When.Hero.DrawsEvent("Unfriendly Eyes")
                .Then().Player.Event.ActiveRow("Spend 1 Secrecy or lose 1 Grace").HasOptions("Spend Secrecy", "Lose Grace")
                .When.Player.SelectsEventOption("Spend Secrecy")
                .Then(Verify.Hero.LostSecrecy());
        }

        [TestCase(1)]
        [TestCase(2)]
        public void LoseGrace(int blightCount)
        {
            var blights = Enumerable.Repeat("Lich", blightCount).ToArray();
            TestScenario
                .Given.Game.WithHero(x => x.At("Village"))
                .Given.Location("Village").Blight(blights)
                .When.Hero.DrawsEvent("Unfriendly Eyes")
                .Then().Player.Event.ActiveRow("Spend 1 Secrecy or lose 1 Grace").HasOptions("Spend Secrecy", "Lose Grace")
                .When.Player.SelectsEventOption("Lose Grace")
                .Then(Verify.Hero.LostGrace());
        }

        [TestCase(3)]
        [TestCase(4)]
        public void SpendGrace(int blightCount)
        {
            var blights = Enumerable.Repeat("Lich", blightCount).ToArray();
            TestScenario
                .Given.Game.WithHero(x => x.At("Village"))
                .Given.Location("Village").Blight(blights)
                .When.Hero.DrawsEvent("Unfriendly Eyes")
                .Then().Player.Event.ActiveRow("Spend 1 Grace or +1 Darkness").HasOptions("Spend Grace", "+1 Darkness")
                .When.Player.SelectsEventOption("Spend Grace")
                .Then(Verify.Hero.LostGrace());
        }

        [TestCase(3)]
        [TestCase(4)]
        public void IncreaseDarkness(int blightCount)
        {
            var blights = Enumerable.Repeat("Lich", blightCount).ToArray();
            TestScenario
                .Given.Game.WithHero(x => x.At("Village"))
                .Given.Location("Village").Blight(blights)
                .When.Hero.DrawsEvent("Unfriendly Eyes")
                .Then().Player.Event.ActiveRow("Spend 1 Grace or +1 Darkness").HasOptions("Spend Grace", "+1 Darkness")
                .When.Player.SelectsEventOption("+1 Darkness")
                .Then().Game.Darkness(1);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void NoSecrecyToSpend(int blightCount)
        {
            var blights = Enumerable.Repeat("Lich", blightCount).ToArray();
            TestScenario
                .Given.Game.WithHero(x => x.At("Village").Secrecy(0))
                .Given.Location("Village").Blight(blights)
                .When.Hero.DrawsEvent("Unfriendly Eyes")
                .Then().Player.Event.HasOptions("Lose Grace");
        }

        [TestCase(3)]
        [TestCase(4)]
        public void NoGraceToSpend(int blightCount)
        {
            var blights = Enumerable.Repeat("Lich", blightCount).ToArray();
            TestScenario
                .Given.Game.WithHero(x => x.At("Village").Grace(0))
                .Given.Location("Village").Blight(blights)
                .When.Hero.DrawsEvent("Unfriendly Eyes")
                .Then().Player.Event.HasOptions("+1 Darkness");
        }
    }
}

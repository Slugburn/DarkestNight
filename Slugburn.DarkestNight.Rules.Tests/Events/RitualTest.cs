using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class RitualTest
    {
        [TestCase(1)]
        [TestCase(2)]
        public void Ritual_BlightCreated(int blightCount)
        {
            var blights = Enumerable.Repeat("Skeletons", blightCount).ToArray();
            var after = blights.Concat(new[] {"Desecration"}).ToArray();
            TestScenario
                .Given.Game.WithHero(h => h.At("Village"))
                .Given.Location("Village").Blights(blights)
                .When.Hero.DrawsEvent("Ritual")
                .Then().Player.EventView.ActiveRow("New blight there").HasOptions("Cancel", "Continue")
                .Given.Game.NextBlight("Desecration")
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Location("Village").Blights(after))
                .Then(Verify.Hero.HasUnresolvedEvents(0));
        }

        [TestCase(3)]
        [TestCase(4)]
        public void Ritual_DarknessIncrease(int blightCount)
        {
            var blights = Enumerable.Repeat("Skeletons", blightCount).ToArray();
            TestScenario
                .Given.Game.WithHero(h => h.At("Village")).Darkness(3)
                .Given.Location("Village").Blights(blights)
                .When.Hero.DrawsEvent("Ritual").Then().Player.EventView
                .ActiveRow("+1 Darkness")
                .HasOptions("Cancel", "Continue")
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Game.Darkness(4))
                .Then(Verify.Hero.HasUnresolvedEvents(0));
        }

        [Test]
        public void Ritual_Cancel()
        {
            TestScenario
                .Given.Game.WithHero()
                .When.Hero.DrawsEvent("Ritual").Then().Player.EventView.HasBody("Ritual", 6,
                    "You may spend 1 Grace and lose 1 Secrecy to cancel this event.\nCount the blights in your location")
                .HasOptions("Cancel", "Continue")
                .When.Player.SelectsEventOption("Cancel")
                .Then(Verify.Hero.HasUnresolvedEvents(0).LostGrace().LostSecrecy());
        }

        [Test]
        public void Ritual_NecromancerMoves()
        {
            TestScenario
                .Given.Game.WithHero(h => h.At("Village")).NecromancerIn("Ruins")
                .Given.Location("Village").Blights()
                .When.Hero.DrawsEvent("Ritual")
                .Then().Player.EventView.ActiveRow("Necromancer moves there").HasOptions("Cancel", "Continue")
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Game.NecromancerAt("Village"))
                .Then(Verify.Hero.HasUnresolvedEvents(0));
        }
    }
}
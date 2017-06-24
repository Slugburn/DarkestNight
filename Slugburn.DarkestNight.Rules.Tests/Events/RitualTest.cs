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
                .Given.Game.WithHero(h => h.Location("Village"))
                .Given.Location("Village").Blight(blights)
                .When.Hero.DrawsEvent("Ritual")
                .Then.Player.Event
                .ActiveRow("New blight there")
                .HasOptions("Cancel", "Continue")
                .Given.Game.NextBlight("Desecration")
                .When.Player.SelectsEventOption("Continue")
                .Then.Location("Village", x => x.Blights(after))
                .Then.Hero(h => h.Event(e => e.HasOutstanding(0)));
        }

        [TestCase(3)]
        [TestCase(4)]
        public void Ritual_DarknessIncrease(int blightCount)
        {
            var blights = Enumerable.Repeat("Skeletons", blightCount).ToArray();
            TestScenario
                .Given.Game.WithHero(h => h.Location("Village")).Darkness(3)
                .Given.Location("Village").Blight(blights)
                .When.Hero.DrawsEvent("Ritual")
                .Then.Player.Event
                .ActiveRow("+1 Darkness")
                .HasOptions("Cancel", "Continue")
                .When.Player.SelectsEventOption("Continue")
                .Then.Game.Darkness(4)
                .Then.Hero(h => h.Event(e => e.HasOutstanding(0)));
        }

        [Test]
        public void Ritual_Cancel()
        {
            TestScenario
                .Given.Game.WithHero()
                .When.Hero.DrawsEvent("Ritual")
                .Then.Player.Event.HasBody("Ritual", 6,
                    "You may spend 1 Grace and lose 1 Secrecy to cancel this event.\nCount the blights in your location")
                .HasOptions("Cancel", "Continue")
                .When.Player.SelectsEventOption("Cancel")
                .Then.Hero(h => h.LostGrace().LostSecrecy().Event(e => e.HasOutstanding(0)));
        }

        [Test]
        public void Ritual_NecromancerMoves()
        {
            TestScenario
                .Given.Game.WithHero(h => h.Location("Village")).NecromancerLocation("Ruins")
                .Given.Location("Village").Blight()
                .When.Hero.DrawsEvent("Ritual")
                .Then.Player.Event.ActiveRow("Necromancer moves there").HasOptions("Cancel", "Continue")
                .When.Player.SelectsEventOption("Continue")
                .Then.Game.NecromancerLocation("Village")
                .Then.Hero(h => h.Event(e => e.HasOutstanding(0)));
        }
    }
}
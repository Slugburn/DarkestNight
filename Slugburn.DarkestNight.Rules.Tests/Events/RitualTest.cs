using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class RitualTest
    {
        [Test]
        public void Ritual_Cancel()
        {
            new TestScenario()
                .GivenHero(x => x.Location("Village"))
                .WhenHero(h => h.DrawsEvent("Ritual"))
                .ThenPlayer(p => p.Event(e => e.HasBody("Ritual", 0,
                        "You may spend 1 Grace and lose 1 Secrecy to cancel this event.\nCount the blights in your location")
                    .HasOptions("Cancel", "Continue")))
                .WhenPlayer(p => p.SelectsEventOption("Cancel"))
                .ThenHero(h => h.LostGrace().LostSecrecy().Event(e => e.HasOutstanding(0)));
        }

        [Test]
        public void Ritual_NecromancerMoves()
        {
            new TestScenario()
                .GivenHero(x => x.Location("Village"))
                .GivenNecromancerLocation("Ruins")
                .WhenHero(h => h.DrawsEvent("Ritual"))
                .ThenPlayer(p => p.Event(e => e.ActiveRow("Necromancer moves there").HasOptions("Cancel", "Continue")))
                .WhenPlayer(p => p.SelectsEventOption("Continue"))
                .ThenNecromancerLocation("Village")
                .ThenHero(h => h.Event(e => e.HasOutstanding(0)));
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Ritual_BlightCreated(int blightCount)
        {
            var blights = Enumerable.Repeat("Skeletons", blightCount).ToArray();
            var after = blights.Concat(new[] {"Desecration"}).ToArray();
            new TestScenario()
                .GivenHero(h => h.Location("Village"))
                .GivenLocation("Village", l => l.Blight(blights))
                .WhenHero(h => h.DrawsEvent("Ritual"))
                .ThenPlayer(p => p.Event(e => e
                    .ActiveRow("New blight there")
                    .HasOptions("Cancel", "Continue")))
                .GivenNextBlight("Desecration")
                .WhenPlayer(p => p.SelectsEventOption("Continue"))
                .ThenSpace("Village", x => x.Blights(after))
                .ThenHero(h => h.Event(e => e.HasOutstanding(0)));
        }

        [TestCase(3)]
        [TestCase(4)]
        public void Ritual_DarknessIncrease(int blightCount)
        {
            var blights = Enumerable.Repeat("Skeletons", blightCount).ToArray();
            new TestScenario()
                .GivenHero(h => h.Location("Village"))
                .GivenDarkness(3)
                .GivenLocation("Village", l => l.Blight(blights))
                .WhenHero(h => h.DrawsEvent("Ritual"))
                .ThenPlayer(p => p.Event(e => e
                    .ActiveRow("+1 Darkness")
                    .HasOptions("Cancel", "Continue")))
                .WhenPlayer(p => p.SelectsEventOption("Continue"))
                .ThenDarkness(4)
                .ThenHero(h => h.Event(e => e.HasOutstanding(0)));
        }
    }
}

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
            TestScenario
                .Given.Game(g => g.Hero())
                .When.Hero(h => h.DrawsEvent("Ritual"))
                .Then.Player(p => p.Event(e => e.HasBody("Ritual", 0,
                        "You may spend 1 Grace and lose 1 Secrecy to cancel this event.\nCount the blights in your location")
                    .HasOptions("Cancel", "Continue")))
                .When.Player(p => p.SelectsEventOption("Cancel"))
                .Then.Hero(h => h.LostGrace().LostSecrecy().Event(e => e.HasOutstanding(0)));
        }

        [Test]
        public void Ritual_NecromancerMoves()
        {
            TestScenario
                .Given.Game(g => g.Hero().NecromancerLocation("Ruins"))
                .When.Hero(h => h.DrawsEvent("Ritual"))
                .Then.Player(p => p.Event(e => e.ActiveRow("Necromancer moves there").HasOptions("Cancel", "Continue")))
                .When.Player(p => p.SelectsEventOption("Continue"))
                .Then.Game(g => g.NecromancerLocation("Village"))
                .Then.Hero(h => h.Event(e => e.HasOutstanding(0)));
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Ritual_BlightCreated(int blightCount)
        {
            var blights = Enumerable.Repeat("Skeletons", blightCount).ToArray();
            var after = blights.Concat(new[] {"Desecration"}).ToArray();
            TestScenario
                .Given.Game(g=>g.Hero(h => h.Location("Village")))
                .Given.Location("Village", l => l.Blight(blights))
                .When.Hero(h => h.DrawsEvent("Ritual"))
                .Then.Player(p => p.Event(e => e
                    .ActiveRow("New blight there")
                    .HasOptions("Cancel", "Continue")))
                .Given.Game(g=>g.NextBlight("Desecration"))
                .When.Player(p => p.SelectsEventOption("Continue"))
                .Then.Location("Village", x => x.Blights(after))
                .Then.Hero(h => h.Event(e => e.HasOutstanding(0)));
        }

        [TestCase(3)]
        [TestCase(4)]
        public void Ritual_DarknessIncrease(int blightCount)
        {
            var blights = Enumerable.Repeat("Skeletons", blightCount).ToArray();
            TestScenario
                .Given.Game(g => g.Hero(h => h.Location("Village")))
                .Given.Game(g => g.Darkness(3))
                .Given.Location("Village", l => l.Blight(blights))
                .When.Hero(h => h.DrawsEvent("Ritual"))
                .Then.Player(p => p.Event(e => e
                    .ActiveRow("+1 Darkness")
                    .HasOptions("Cancel", "Continue")))
                .When.Player(p => p.SelectsEventOption("Continue"))
                .Then.Game(g => g.Darkness(4))
                .Then.Hero(h => h.Event(e => e.HasOutstanding(0)));
        }
    }
}

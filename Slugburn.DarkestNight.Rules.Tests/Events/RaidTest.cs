using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class RaidTest
    {
        [TestCase(0, "Lose 2 Secrecy", 0, 2, 0)]
        [TestCase(1, "Lose 2 Secrecy", 0, 2, 0)]
        [TestCase(2, "Lose 1 Grace and 1 Secrecy", 1, 1, 0)]
        [TestCase(3, "Lose 1 Grace and 1 Secrecy", 1, 1, 0)]
        [TestCase(4, "+1 Darkness", 0, 0, 1)]
        public void Raid(int blightCount, string rowText, int lostGrace, int lostSecrecy, int darkness)
        {
            var blights = Enumerable.Repeat("Lich", blightCount).ToArray();
            new TestScenario()
                .GivenHero(x => x.Location("Forest"))
                .GivenLocation("Forest", x => x.Blight(blights))
                .WhenHero(x => x.DrawsEvent("Raid"))
                .ThenPlayer(p => p.Event(e => e
                    .HasBody("Raid", 6, "Count the blights in your location")
                    .HasOptions("Continue")
                    .ActiveRow(rowText)))
                .WhenPlayer(x => x.SelectsEventOption("Continue"))
                .ThenHero(x => x.LostSecrecy(lostSecrecy).LostGrace(lostGrace))
                .ThenDarkness(darkness);
        }
    }
}

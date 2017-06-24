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
            TestScenario
                .Given.Game.Hero(x => x.Location("Forest"))
                .Given.Location("Forest", x => x.Blight(blights))
                .When.Hero.DrawsEvent("Raid")
                .Then.Player(p => p.Event(e => e
                    .HasBody("Raid", 6, "Count the blights in your location")
                    .HasOptions("Continue")
                    .ActiveRow(rowText)))
                .When.Player.SelectsEventOption("Continue")
                .Then.Hero(x => x.LostSecrecy(lostSecrecy).LostGrace(lostGrace))
                .Then.Game(g => g.Darkness(darkness));
        }
    }
}

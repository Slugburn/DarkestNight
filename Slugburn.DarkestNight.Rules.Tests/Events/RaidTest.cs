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
                .Game.WithHero().At("Forest")
                .Given.Location("Forest").Blights(blights)
                .When.Hero.DrawsEvent("Raid")
                .Then(Verify.Player.EventView
                    .HasBody("Raid", 6, "Count the blights in your location")
                    .HasOptions("Continue")
                    .ActiveRow(rowText))
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Hero.LostSecrecy(lostSecrecy).LostGrace(lostGrace))
                .Then(Verify.Game.Darkness(darkness));
        }
    }
}
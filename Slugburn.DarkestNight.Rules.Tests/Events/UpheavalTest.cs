using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class UpheavalTest
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void Upheaval(int blightCount)
        {
            var blights = Enumerable.Repeat("Confusion", blightCount).ToArray();
            var newBlights = Enumerable.Repeat("Desecration", blightCount).ToArray();
            TestScenario
                .Given.Game.WithHero(x => x.At("Village"))
                .Given.Location("Village").Blights(blights)
                .When.Hero.DrawsEvent("Upheaval")
                .Then().Player.EventView
                    .HasBody("Upheaval", 2, "Remove all blights from your current location and create an equal number of new blights.")
                    .HasOptions("Continue")
                .Given.Game.NextBlight(newBlights)
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Location("Village").Blights(newBlights));
        }
    }
}

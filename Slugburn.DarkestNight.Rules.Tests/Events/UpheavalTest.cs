using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                .Then().Player.Event
                    .HasBody("Upheaval", 2, "Remove all blights from your current location and create an equal number of new blights.")
                    .HasOptions("Continue")
                .Given.Game.NextBlight(newBlights)
                .When.Player.SelectsEventOption("Continue")
                .Then().Location("Village", x => x.Blights(newBlights));
        }
    }
}

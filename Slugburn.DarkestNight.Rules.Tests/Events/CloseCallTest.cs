using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class CloseCallTest
    {
        [TestCase(6, "No effect", 0, 0)]
        [TestCase(5, "No effect", 0, 0)]
        [TestCase(4, "Lose 1 Secrecy", 1, 0)]
        [TestCase(3, "Lose 1 Secrecy", 1, 0)]
        [TestCase(2, "Lose 1 Grace", 0, 1)]
        [TestCase(1, "Lose 1 Grace", 0, 1)]
        public void CloseCall(int roll, string effect, int lostSecrecy, int lostGrace)
        {
            TestScenario
                .Given.Game.Hero()
                .When.Hero.DrawsEvent("Close Call")
                .Then.Player(p => p.Event(e => e.HasBody("Close Call", 4, "Roll 1d and take the highest").HasOptions("Roll")))
                .When.Player.SelectsEventOption("Roll", x => x.Rolls(roll))
                .Then.Player(p => p.Event(e => e.ActiveRow(effect)))
                .When.Player.SelectsEventOption("Continue")
                .Then.Hero(h => h.LostSecrecy(lostSecrecy).LostGrace(lostGrace));
        }
    }
}

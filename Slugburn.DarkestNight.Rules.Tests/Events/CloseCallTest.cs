using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class CloseCallTest
    {
        [TestCase(5,6, "No effect", 0, 0)]
        [TestCase(3,4, "Lose 1 Secrecy", 1, 0)]
        [TestCase(1,2, "Lose 1 Grace", 0, 1)]
        public void CloseCall(int min, int max, string effect, int lostSecrecy, int lostGrace)
        {
            foreach (var roll in Enumerable.Range(min, max - min + 1))
            {
                new TestScenario()
                    .GivenHero("Acolyte")
                    .WhenHero(x => x.DrawsEvent("Close Call"))
                    .ThenPlayer(p => p.Event(e => e.Body("Close Call", "Roll 1d and take the highest", 4).Option("Roll")))
                    .WhenPlayer(p => p.SelectsEventOption("Roll", x => x.Rolls(roll)))
                    .ThenPlayer(p => p.Event(e => e.ActiveRow(min, max, effect)))
                    .WhenPlayer(p => p.SelectsEventOption("Continue"))
                    .ThenHero(h => h.LostSecrecy(lostSecrecy).LostGrace(lostGrace));
            }
        }
    }
}

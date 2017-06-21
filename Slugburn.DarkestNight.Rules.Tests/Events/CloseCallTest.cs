using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

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
            new TestScenario()
                .GivenHero("Acolyte")
                .WhenHero(x => x.DrawsEvent("Close Call"))
                .ThenPlayer(p => p.SeesEvent("Close Call", "Roll 1d and take the highest", 4, "Roll"))
                .WhenPlayer(p => p.SelectsEventOption("Roll", x => x.Rolls(roll)).AcceptsRoll())
                .ThenPlayer(p => p.SeesEvent("Close Call", effect, 4, "Continue"))
                .WhenPlayer(p => p.SelectsEventOption("Continue"))
                .ThenHero(h => h.LostSecrecy(lostSecrecy).LostGrace(lostGrace));
        }
    }
}

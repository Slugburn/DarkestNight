using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class DarkScryingTest
    {
        [TestCase("Spend Grace", 1, 0)]
        [TestCase("Lose Secrecy", 0, 2)]
        public void CloseCall_GraceAvailable(string option, int lostGrace, int lostSecrecy)
        {
            new TestScenario()
                .GivenHero("Acolyte")
                .WhenHero(x => x.DrawsEvent("Dark Scrying"))
                .ThenPlayer(p => p.Event(e => e.HasBody("Dark Scrying", 4, "Spend 1 Grace or lose 2 Secrecy.").HasOptions("Spend Grace", "Lose Secrecy")))
                .WhenPlayer(p => p.SelectsEventOption(option))
                .ThenHero(h => h.LostSecrecy(lostSecrecy).LostGrace(lostGrace));
        }

       [Test]
        public void CloseCall_NoGraceAvailable()
        {
            new TestScenario()
                .GivenHero("Acolyte", x=>x.Grace(0))
                .WhenHero(x => x.DrawsEvent("Dark Scrying"))
                .ThenPlayer(p => p.Event(e => e.HasBody("Dark Scrying", 4, "Spend 1 Grace or lose 2 Secrecy.").HasOptions("Lose Secrecy")))
                .WhenPlayer(p => p.SelectsEventOption("Lose Secrecy"))
                .ThenHero(h => h.Grace(0).LostSecrecy(2));
        }
    }
}
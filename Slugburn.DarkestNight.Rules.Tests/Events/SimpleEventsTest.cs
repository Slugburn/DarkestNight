using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class SimpleEventsTest
    {
        [TestCase]
        public void Midnight()
        {
            new TestScenario()
                .GivenHero()
                .GivenDarkness(3)
                .WhenHero(h => h.DrawsEvent("Midnight"))
                .WhenPlayer(p => p.SelectsEventOption("Continue"))
                .ThenDarkness(4);
        }
    }
}

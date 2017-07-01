using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Blights
{
    [TestFixture]
    public class DarkFogTest
    {
        [Test]
        public void Effect()
        {
            // The search difficulty at the affected location is increased by 2
            TestScenario.Game
                .Location("Village").HasBlights("DarkFog")
                .Then(Verify.Location("Village").Blights("DarkFog").SearchTarget(5));
        }

        [Test]
        public void Defense()
        {
            // Lose a turn.
        }
    }
}

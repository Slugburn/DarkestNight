using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Blights
{
    [TestFixture]
    public class DesecrationTest
    {
        [Test]
        public void Effect()
        {
            // The Darkness increases one extra point at the start of each Necromancer turn.
            TestScenario.Game
                .Darkness(5)
                .Location("Village").HasBlights("Desecration")
                .Given.Game.Necromancer.IsTakingTurn()
                .Then(Verify.Game.Darkness(7));
        }
    }
}

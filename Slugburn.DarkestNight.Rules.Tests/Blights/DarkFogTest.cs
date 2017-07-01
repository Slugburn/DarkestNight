using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

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
            // Defense: Lose a turn.
            TestScenario.Game
                .WithHero().At("Village")
                .Location("Village").HasBlights("DarkFog")
                .When.Player.TakesAction("Attack").Fights(Fake.Rolls(1))
                .Given.Game.NewDay()
                .Then(Verify.Hero().CanTakeAction("Start Turn", false));
        }
    }
}

using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Actions
{
    [TestFixture]
    public class AttackTest
    {
        [Test]
        public void ThereMustBeABlightAtTheHeroLocation()
        {
            TestScenario.Game
                .WithHero().At("Village")
                .Location("Village").Blights()
                .Then(Verify.Hero().CanTakeAction("Attack", false));
        }
    }
}

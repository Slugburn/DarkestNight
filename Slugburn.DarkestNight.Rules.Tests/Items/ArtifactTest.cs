using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Items
{
    [TestFixture]
    public class ArtifactTest
    {
        [Test]
        public void BloodRing()
        {
            TestScenario.Game
                .WithHero().HasItems("Blood Ring")
                .Then(Verify.Hero().FightDice(2));
        }
    }
}

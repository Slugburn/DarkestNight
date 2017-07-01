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

        [Test]
        public void CrystalBall()
        {
            TestScenario.Game
                .WithHero().HasItems("Crystal Ball")
                .Then(Verify.Hero().SearchDice(2));
        }

        [Test]
        public void GhostMail()
        {
            TestScenario.Game
                .WithHero().HasItems("Ghost Mail")
        }
    }
}

using System;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Items
{
    [TestFixture]
    public class ArtifactTest
    {
        // 1d in fights.
        [Test]
        public void BloodRing()
        {
            TestScenario.Game
                .WithHero().HasItems("Blood Ring")
                .Then(Verify.Hero().FightDice(2));
        }

        // +1d in searches.
        [Test]
        public void CrystalBall()
        {
            TestScenario.Game
                .WithHero().HasItems("Crystal Ball")
                .Then(Verify.Hero().SearchDice(2));
        }

        // +1d when eluding.
        [Test]
        public void MagicMask()
        {
            TestScenario.Game
                .WithHero().HasItems("Magic Mask")
                .Then(Verify.Hero().EludeDice(2));
        }

        // You may ignore the effect of one blight each turn.
        [Test]
        public void VanishingCowl()
        {
            TestScenario.Game
                .WithHero().At("Village").HasItems("Vanishing Cowl")
                .Location("Castle").HasBlights("Curse")
                .When.Player.TakesAction("Vanishing Cowl").SelectsBlight("Castle", "Curse")
                .When.Player.TakesAction("Travel").SelectsLocation("Castle")
                .Then(Verify.Player.Hero().LostGrace(0));
        }
    }
}

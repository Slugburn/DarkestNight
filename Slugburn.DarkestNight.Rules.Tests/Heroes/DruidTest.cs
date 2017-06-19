using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class DruidTest
    {
        [TestCase(false)]
        [TestCase(true)]
        public void AnimalCompanion(bool attackSucceeds)
        {
            var roll = attackSucceeds ? new[] {1, 6} : new[] {3, 4};
            var expectedBlights = attackSucceeds ? new Blight[0] : new[] {Blight.Corruption};
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Animal Companion").Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Corruption))
                .WhenPlayerTakesAttackAction(x => x.Tactic("Animal Companion").Rolls(roll))
                .ThenSpace(Location.Village, x => x.Blights(expectedBlights))
                .ThenPlayer(x => x.RolledNumberOfDice(2))
                .ThenHero(x => x.HasUsedAction().LostSecrecy())
                .ThenPower("Animal Companion", x=>x.IsExhausted(!attackSucceeds));
        }

        [Test]
        public void Camouflage()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Camouflage").Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Skeletons))
                .WhenPlayerEludes(x => x.Tactic("Camouflage").Rolls(1, 6))
                .ThenPlayer(x=>x.RolledNumberOfDice(2))
                .ThenHero(x=>x.LostGrace(0));
        }

        [Test]
        public void Celerity()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Celerity", "Raven Form", "Wolf Form").Location(Location.Monastery))
                .GivenPower("Wolf Form", x => x.IsActive())
                .WhenPlayerTakesAction("Celerity")
                .ThenPower("Wolf Form", x => x.IsActive(false))
                .WhenPlayerSelectsLocation(Location.Village)
                .ThenHero(x => x.Location(Location.Village))
                .ThenAvailableActions("Raven Form", "Wolf Form", "Continue")
                .WhenPlayerTakesAction("Raven Form")
                .ThenPower("Raven Form", x => x.IsActive());
        }

        [Test]
        public void Celerity_NoNewFormSelected()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Celerity").Location(Location.Monastery))
                .WhenPlayerTakesAction("Celerity")
                .WhenPlayerSelectsLocation(Location.Village)
                .ThenHero(x => x.Location(Location.Village))
                .ThenAvailableActions("Continue")
                .WhenPlayerTakesAction("Continue")
                .ThenHero(x=>x.HasUsedAction());
        }

        [Test]
        public void SpriteFormActivated()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Sprite Form", "Raven Form"))
                .GivenPower("Raven Form", x => x.IsActive())
                .WhenPlayerTakesAction("Sprite Form")
                .ThenHero(x=>x.HasUsedAction())
                .ThenPower("Sprite Form", x => x.IsActive())
                .ThenPower("Raven Form", x => x.IsActive(false));
        }

        [Test]
        public void SpriteForm_IgnoreBlightsWhileActive()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Sprite Form"))
                .GivenPower("Sprite Form", x=>x.IsActive())
                .ThenHero(x=>x.IsIgnoringBlights());
        }

        [Test]
        public void SpriteForm_DoesNotIgnoreBlightsWhenNecromancerIsPresent()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Sprite Form").Location(Location.Ruins))
                .GivenNecromancerLocation(Location.Ruins)
                .GivenPower("Sprite Form", x => x.IsActive())
                .ThenHero(x => x.IsNotIgnoringBlights());
        }

        [Test]
        public void SpriteForm_Deactivate()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Sprite Form"))
                .GivenPower("Sprite Form", x => x.IsActive())
                .WhenPlayerTakesAction("Deactivate Form")
                .ThenHero(x => x.IsNotIgnoringBlights().HasUsedAction());
        }
    }
}

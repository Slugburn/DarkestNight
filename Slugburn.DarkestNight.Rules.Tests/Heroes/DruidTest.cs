using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

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
            var expectedBlights = attackSucceeds ? new string[0] : new[] {"Corruption"};
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Animal Companion").Location("Village"))
                .GivenLocation("Village", x => x.Blight("Corruption"))
                .WhenPlayerTakesAttackAction(x => x.Tactic("Animal Companion").Rolls(roll))
                .ThenSpace("Village", x => x.Blights(expectedBlights))
                .ThenHero(x => x.RolledNumberOfDice(2).HasUsedAction().LostSecrecy())
                .ThenPower("Animal Companion", x=>x.IsExhausted(!attackSucceeds));
        }

        [Test]
        public void Camouflage()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Camouflage").Location("Village"))
                .GivenLocation("Village", x => x.Blight("Skeletons"))
                .WhenHero(h => h.Eludes(x => x.Tactic("Camouflage").Rolls(1, 6)))
                .ThenHero(x => x.RolledNumberOfDice(2).LostGrace(0));
        }

        [Test]
        public void Celerity()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Celerity", "Raven Form", "Wolf Form").Location("Monastery"))
                .GivenPower("Wolf Form", x => x.IsActive())
                .WhenPlayerTakesAction("Celerity")
                .ThenPower("Wolf Form", x => x.IsActive(false))
                .WhenPlayer(x=>x.SelectsLocation("Village"))
                .ThenHero(x => x.Location("Village").HasAvailableActions("Raven Form", "Wolf Form", "Continue"))
                .WhenPlayerTakesAction("Raven Form")
                .ThenPower("Raven Form", x => x.IsActive());
        }

        [Test]
        public void Celerity_NoNewFormSelected()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Celerity").Location("Monastery"))
                .WhenPlayerTakesAction("Celerity")
                .WhenPlayer(x => x.SelectsLocation("Village"))
                .ThenHero(x => x.Location("Village").HasAvailableActions("Continue"))
                .WhenPlayerTakesAction("Continue")
                .ThenHero(x => x.HasUsedAction());
        }

        [Test]
        public void RavenForm_Activate()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Sprite Form", "Raven Form"))
                .GivenPower("Sprite Form", x => x.IsActive())
                .WhenPlayerTakesAction("Raven Form")
                .ThenHero(x => x.TravelSpeed(2).SearchDice(2).CanGainGrace(false).HasUsedAction())
                .ThenPower("Raven Form", x => x.IsActive())
                .ThenPower("Sprite Form", x => x.IsActive(false));
        }

        [Test]
        public void RavenForm_Deactivate()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Raven Form"))
                .GivenPower("Raven Form", x => x.IsActive())
                .WhenPlayerTakesAction("Deactivate Form")
                .ThenHero(x=>x.TravelSpeed(1).SearchDice(1).HasUsedAction());
        }

        [Test]
        public void SpriteFormActivated()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Sprite Form", "Raven Form"))
                .GivenPower("Raven Form", x => x.IsActive())
                .WhenPlayerTakesAction("Sprite Form")
                .ThenHero(x=>x.CanGainGrace(false).HasUsedAction())
                .ThenPower("Sprite Form", x => x.IsActive())
                .ThenPower("Raven Form", x => x.IsActive(false));
        }

        [Test]
        public void SpriteForm_IgnoreBlightsWhileActive()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Sprite Form"))
                .GivenPower("Sprite Form", x=>x.IsActive())
                .ThenHero(x=>x.CanGainGrace(false).IsIgnoringBlights());
        }

        [Test]
        public void SpriteForm_DoesNotIgnoreBlightsWhenNecromancerIsPresent()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Sprite Form").Location("Ruins"))
                .GivenNecromancerLocation("Ruins")
                .GivenPower("Sprite Form", x => x.IsActive())
                .ThenHero(x => x.CanGainGrace(false).IsNotIgnoringBlights());
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

        [Test]
        public void Tranquility()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Tranquility"))
                .ThenHero(x => x.DefaultGrace(8).Grace(5));
        }

        [Test]
        public void TreeForm_Activate()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Tree Form", "Wolf Form"))
                .GivenPower("Wolf Form", x => x.IsActive())
                .WhenPlayerTakesAction("Tree Form")
                .ThenHero(x => x.HasUsedAction())
                .ThenPower("Tree Form", x => x.IsActive())
                .ThenPower("Wolf Form", x => x.IsActive(false));
        }

        [Test]
        public void TreeForm_GainTwoGraceAtStartOfTurn()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Tree Form").Grace(0))
                .GivenPower("Tree Form", x => x.IsActive())
                .WhenHero(x=>x.StartsTurn())
                .ThenHero(x=>x.Grace(2));
        }

        [Test]
        public void TreeForm_MaxAtDefaultGrace()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Tree Form").Grace(4))
                .GivenPower("Tree Form", x => x.IsActive())
                .WhenHero(x => x.StartsTurn())
                .ThenHero(x => x.DefaultGrace(5).Grace(5));
        }

        [Test]
        public void TreeForm_RestrictedActions()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Tree Form", "Celerity", "Raven Form", "Sprite Form", "Wolf Form"))
                .GivenPower("Tree Form", x => x.IsActive())
                .WhenHero(x=>x.StartsTurn())
                .ThenHero(x=>x.HasAvailableActions("Hide", "Tree Form", "Celerity", "Raven Form", "Sprite Form", "Wolf Form", "Deactivate Form"));
        }

        [Test]
        public void TreeForm_Deactivate()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Tree Form").Grace(0).Location("Monastery"))
                .GivenPower("Tree Form", x => x.IsActive())
                .WhenPlayerTakesAction("Deactivate Form")
                .WhenHero(x=>x.StartsTurn())
                .ThenHero(x => x.Grace(0).HasAvailableActions("Travel", "Hide", "Pray", "Tree Form"));
        }

        [Test]
        public void Vines_Fight()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Vines").Location("Mountains"))
                .GivenLocation("Mountains", x => x.Blight("Zombies"))
                .WhenHero(h => h.Fights(x=>x.Tactic("Vines [Fight]").Rolls(2, 3, 4, 5)))
                .ThenHero(x=>x.RolledNumberOfDice(4))
                .ThenPower("Vines", x => x.IsExhausted());
        }

        [Test]
        public void Vines_Elude()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Vines").Location("Mountains"))
                .GivenLocation("Mountains", x => x.Blight("Zombies"))
                .WhenHero(h => h.Eludes(x => x.Tactic("Vines [Elude]").Rolls(1, 2, 3, 4)))
                .ThenHero(x => x.RolledNumberOfDice(4))
                .ThenPower("Vines", x => x.IsExhausted());
        }

        [Test]
        public void Visions_IgnoreEvent()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Visions"))
                .WhenHero(x=>x.DrawsEvent("Anathema"))
                .ThenPlayer(x => x.SeesEvent("Anathema", "Lose 1 Grace.", 6, "Continue", "Ignore [Visions]"))
                .WhenPlayer(x => x.SelectsEventOption("Ignore [Visions]"))
                .ThenHero(x => x.LostGrace(0)) // Anathema causes hero to lose 1 Grace unless ignored
                .ThenPower("Visions", x => x.IsExhausted());
        }

        [Test]
        public void Visions_CannotIgnoreRenewal()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Visions"))
                .WhenHero(x => x.DrawsEvent("Renewal"))
                .ThenPlayer(x => x.SeesEvent("Renewal", "Reshuffle the Event Deck and draw another card.", 0, "Continue"));
        }

        [Test]
        public void WolfForm_Activate()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Wolf Form"))
                .WhenPlayerTakesAction("Wolf Form")
                .ThenPower("Wolf Form", x => x.IsActive())
                .ThenHero(x=>x.HasUsedAction().CanGainGrace(false).FightDice(2).EludeDice(2));
        }
    }
}

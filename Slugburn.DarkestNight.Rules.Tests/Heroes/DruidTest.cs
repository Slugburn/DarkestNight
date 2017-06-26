using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class DruidTest
    {
        // Animal Companion (Tactic): Fight with 2 dice. Exhaust if you fail.
        [TestCase(false)]
        [TestCase(true)]
        public void AnimalCompanion(bool attackSucceeds)
        {
            var roll = attackSucceeds ? new[] {1, 6} : new[] {3, 4};
            TestScenario.Given.Game
                .WithHero("Druid").HasPowers("Animal Companion")
                .When.Hero.FacesEnemy("Zombie")
                .When.Player.ResolvesConflict(x => x.Tactic("Animal Companion").Target("Zombie").Rolls(roll)).AcceptsRoll().AcceptsConflictResults()
                .Then(Verify.Hero.RolledNumberOfDice(2).WasWounded(!attackSucceeds))
                .Then(Verify.Power("Animal Companion").IsExhausted(!attackSucceeds));
        }

        [Test]
        public void Camouflage()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.HasPowers("Camouflage").At("Village"))
                .GivenLocation("Village", x => x.Blights("Skeletons"))
                .WhenHero(h => h.Eludes(x => x.Tactic("Camouflage").Rolls(1, 6)))
                .ThenHero(x => x.RolledNumberOfDice(2).LostGrace(0));
        }

        [Test]
        public void Celerity()
        {
            TestScenario
                .Given.Game.WithHero("Druid").HasPowers("Celerity", "Raven Form", "Wolf Form").At("Monastery")
                .Power("Wolf Form").IsActive()
                .When.Player.TakesAction("Celerity")
                .Then(Verify.Power("Wolf Form").IsActive(false))
                .When.Player.SelectsLocation("Village")
                .Then(Verify.Hero.Location("Village").HasAvailableActions("Raven Form", "Wolf Form", "Continue"))
                .When.Player.TakesAction("Raven Form")
                .Then(Verify.Power("Raven Form").IsActive())
                .Then(Verify.Hero.TravelSpeed(2).SearchDice(2).HasUsedAction().CanGainGrace(false));
        }

        [Test]
        public void Celerity_NoNewFormSelected()
        {
            TestScenario
                .Given.Game.WithHero("Druid").HasPowers("Celerity").At("Monastery")
                .When.Player.TakesAction("Celerity")
                .When.Player.SelectsLocation("Village")
                .Then(Verify.Hero.Location("Village").HasAvailableActions("Continue"))
                .When.Player.TakesAction("Continue")
                .Then(Verify.Hero.HasUsedAction());
        }

        [Test]
        public void RavenForm_Activate()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.HasPowers("Sprite Form", "Raven Form"))
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
                .GivenHero("Druid", x => x.HasPowers("Raven Form"))
                .GivenPower("Raven Form", x => x.IsActive())
                .WhenPlayerTakesAction("Deactivate Form")
                .ThenHero(x => x.TravelSpeed(1).SearchDice(1).HasUsedAction());
        }

        [Test]
        public void SpriteForm_Deactivate()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.HasPowers("Sprite Form"))
                .GivenPower("Sprite Form", x => x.IsActive())
                .WhenPlayerTakesAction("Deactivate Form")
                .ThenHero(x => x.IsNotIgnoringBlights().HasUsedAction());
        }

        [Test]
        public void SpriteForm_DoesNotIgnoreBlightsWhenNecromancerIsPresent()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.HasPowers("Sprite Form").At("Ruins"))
                .GivenNecromancerLocation("Ruins")
                .GivenPower("Sprite Form", x => x.IsActive())
                .ThenHero(x => x.CanGainGrace(false).IsNotIgnoringBlights());
        }

        [Test]
        public void SpriteForm_IgnoreBlightsWhileActive()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.HasPowers("Sprite Form"))
                .GivenPower("Sprite Form", x => x.IsActive())
                .ThenHero(x => x.CanGainGrace(false).IsIgnoringBlights());
        }

        [Test]
        public void SpriteFormActivated()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.HasPowers("Sprite Form", "Raven Form"))
                .GivenPower("Raven Form", x => x.IsActive())
                .WhenPlayerTakesAction("Sprite Form")
                .ThenHero(x => x.CanGainGrace(false).HasUsedAction())
                .ThenPower("Sprite Form", x => x.IsActive())
                .ThenPower("Raven Form", x => x.IsActive(false));
        }

        [Test]
        public void Tranquility()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.HasPowers("Tranquility"))
                .ThenHero(x => x.DefaultGrace(8).Grace(5));
        }

        [Test]
        public void TreeForm_Activate()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.HasPowers("Tree Form", "Wolf Form"))
                .GivenPower("Wolf Form", x => x.IsActive())
                .WhenPlayerTakesAction("Tree Form")
                .ThenHero(x => x.HasUsedAction())
                .ThenPower("Tree Form", x => x.IsActive())
                .ThenPower("Wolf Form", x => x.IsActive(false));
        }

        [Test]
        public void TreeForm_Deactivate()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.HasPowers("Tree Form").Grace(0).At("Monastery"))
                .GivenPower("Tree Form", x => x.IsActive())
                .WhenPlayerTakesAction("Deactivate Form")
                .WhenHero(x => x.StartsTurn())
                .ThenHero(x => x.Grace(0).HasAvailableActions("Travel", "Hide", "Pray", "Tree Form", "End Turn"));
        }

        [Test]
        public void TreeForm_GainTwoGraceAtStartOfTurn()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.HasPowers("Tree Form").Grace(0))
                .GivenPower("Tree Form", x => x.IsActive())
                .WhenHero(x => x.StartsTurn())
                .ThenHero(x => x.Grace(2));
        }

        [Test]
        public void TreeForm_MaxAtDefaultGrace()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.HasPowers("Tree Form").Grace(4))
                .GivenPower("Tree Form", x => x.IsActive())
                .WhenHero(x => x.StartsTurn())
                .ThenHero(x => x.DefaultGrace(5).Grace(5));
        }

        [Test]
        public void TreeForm_RestrictedActions()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.HasPowers("Tree Form", "Celerity", "Raven Form", "Sprite Form", "Wolf Form"))
                .GivenPower("Tree Form", x => x.IsActive())
                .WhenHero(x => x.StartsTurn())
                .ThenHero(x => x.HasAvailableActions("Hide", "Tree Form", "Celerity", "Raven Form", "Sprite Form", "Wolf Form", "Deactivate Form"));
        }

        [Test]
        public void Vines_Elude()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.HasPowers("Vines").At("Mountains"))
                .GivenLocation("Mountains", x => x.Blights("Zombies"))
                .WhenHero(h => h.Eludes(x => x.Tactic("Vines [Elude]").Rolls(1, 2, 3, 4)))
                .ThenHero(x => x.RolledNumberOfDice(4))
                .ThenPower("Vines", x => x.IsExhausted());
        }

        [Test]
        public void Vines_Fight()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.HasPowers("Vines").At("Mountains"))
                .GivenLocation("Mountains", x => x.Blights("Zombies"))
                .WhenHero(h => h.Fights(x => x.Tactic("Vines [Fight]").Rolls(2, 3, 4, 5)))
                .ThenHero(x => x.RolledNumberOfDice(4))
                .ThenPower("Vines", x => x.IsExhausted());
        }

        [Test]
        public void Visions_CannotIgnoreRenewal()
        {
            TestScenario
                .Given.Game.WithHero("Druid").HasPowers("Visions")
                .When.Hero.DrawsEvent("Renewal")
                .Then(Verify.Hero.CurrentEvent.CanBeIgnored(false))
                .Then(Verify.Player.EventView.HasOptions("Continue"));
        }

        [Test]
        public void Visions_IgnoreEvent()
        {
            TestScenario
                .Given.Game.WithHero("Druid").HasPowers("Visions")
                .When.Hero.DrawsEvent("Anathema")
                .Then(Verify.Player.EventView.HasOptions("Continue", "Ignore [Visions]"))
                .When.Player.SelectsEventOption("Ignore [Visions]")
                .Then(Verify.Hero.LostGrace(0)) // Anathema causes hero to lose 1 Grace unless ignored
                .Then(Verify.Power("Visions").IsExhausted());
        }

        [Test]
        public void WolfForm_Activate()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.HasPowers("Wolf Form"))
                .WhenPlayerTakesAction("Wolf Form")
                .ThenPower("Wolf Form", x => x.IsActive())
                .ThenHero(x => x.HasUsedAction().CanGainGrace(false).FightDice(2).EludeDice(2));
        }
    }
}
﻿using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

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
                .When.Player.CompletesConflict("Zombie", "Animal Companion", Fake.Rolls(roll))
                .Then(Verify.Hero.RolledNumberOfDice(2).WasWounded(!attackSucceeds))
                .Then(Verify.Power("Animal Companion").IsExhausted(!attackSucceeds));
        }

        // Camouflage (Tactic): Elude with 2 dice.
        [Test]
        public void Camouflage()
        {
            TestScenario.Given.Game
                .WithHero("Druid").HasPowers("Camouflage")
                .When.Hero.FacesEnemy("Zombie")
                .When.Player.CompletesConflict("Zombie", "Camouflage", Fake.Rolls(1, 6))
                .Then(Verify.Hero.RolledNumberOfDice(2).WasWounded(false));
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
            TestScenario.Given.Game
                .NecromancerAt("Ruins")
                .WithHero("Druid").HasPowers("Sprite Form").At("Ruins")
                .Given.ActingHero().Power("Sprite Form").IsActive()
                .Then(Verify.Hero.CanGainGrace(false).IsNotIgnoringBlights());
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

        // Tree Form (Action): Deactivate all Forms. Optionally activate.
        // Gain 2 Grace (up to default) at the start of your turn. Your actions can only be to hide or use a Druid power.
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
            TestScenario.Given.Game
                .WithHero("Druid").HasPowers("Tree Form").Grace(0)
                .Power("Tree Form").IsActive()
                .When.Player.TakesAction("Deactivate Form")
                .When.Player.StartsTurn()
                .Then(Verify.Hero.Grace(0).HasAvailableActions("Travel", "Hide", "Pray", "Tree Form", "End Turn"));
        }

        [Test]
        public void TreeForm_GainTwoGraceAtStartOfTurn()
        {
            TestScenario.Given.Game
                .WithHero("Druid").HasPowers("Tree Form").Grace(0)
                .Power("Tree Form").IsActive()
                .When.Player.StartsTurn()
                .Then(Verify.Hero.Grace(2));
        }

        [Test]
        public void TreeForm_MaxAtDefaultGrace()
        {
            TestScenario.Given.Game
                .WithHero("Druid").HasPowers("Tree Form").Grace(4)
                .Power("Tree Form").IsActive()
                .When.Player.StartsTurn()
                .Then(Verify.Hero.DefaultGrace(5).Grace(5));
        }

        [Test]
        public void TreeForm_RestrictedActions()
        {
            TestScenario.Given.Game
                .WithHero("Druid").HasPowers("Tree Form", "Celerity", "Raven Form", "Sprite Form", "Wolf Form")
                .Power("Tree Form").IsActive()
                .When.Player.StartsTurn()
                .Then(Verify.Hero.HasAvailableActions("Hide", "Tree Form", "Celerity", "Raven Form", "Sprite Form", "Wolf Form", "Deactivate Form"));
        }

        // Vines (Tactic): Exhaust to fight or elude with 4 dice.
        [Test]
        public void Vines_Fight()
        {
            TestScenario.Given.Game
                .WithHero("Druid").HasPowers("Vines")
                .When.Hero.FacesEnemy("Zombie")
                .When.Player.CompletesConflict("Zombie", "Vines [fight]", Fake.Rolls(1, 2, 3, 4))
                .Then(Verify.Hero.RolledNumberOfDice(4).WasWounded())
                .Then(Verify.Power("Vines").IsExhausted());
        }

        [Test]
        public void Vines_Elude()
        {
            TestScenario.Given.Game
                .WithHero("Druid").HasPowers("Vines")
                .When.Hero.FacesEnemy("Zombie")
                .When.Player.CompletesConflict("Zombie", "Vines [elude]", Fake.Rolls(1, 2, 3, 4))
                .Then(Verify.Hero.RolledNumberOfDice(4))
                .Then(Verify.Power("Vines").IsExhausted());
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
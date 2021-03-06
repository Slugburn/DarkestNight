﻿using NUnit.Framework;
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
            TestScenario.Game
                .WithHero("Druid").HasPowers("Animal Companion")
                .Given.Hero().IsFacingEnemy("Zombie")
                .When.Player.CompletesConflict("Zombie", "Animal Companion", Fake.Rolls(roll))
                .Then(Verify.Hero().RolledNumberOfDice(2).WasWounded(!attackSucceeds))
                .Then(Verify.Power("Animal Companion").IsExhausted(!attackSucceeds));
        }

        // Camouflage (Tactic): Elude with 2 dice.
        [Test]
        public void Camouflage()
        {
            TestScenario.Game
                .WithHero("Druid").HasPowers("Camouflage")
                .Given.Hero().IsFacingEnemy("Zombie")
                .When.Player.CompletesConflict("Zombie", "Camouflage", Fake.Rolls(1, 6))
                .Then(Verify.Hero().RolledNumberOfDice(2).WasWounded(false));
        }

        [Test]
        public void Celerity()
        {
            TestScenario
                .Game.WithHero("Druid").HasPowers("Celerity", "Raven Form", "Wolf Form").At("Monastery")
                .Power("Wolf Form").IsActive()
                .When.Player.TakesAction("Celerity")
                .Then(Verify.Power("Wolf Form").IsActive(false))
                .When.Player.SelectsLocation("Village")
                .Then(Verify.Hero().Location("Village").HasFreeAction().HasAvailableActions("Raven Form", "Wolf Form", "Skip Free Action"))
                .When.Player.TakesAction("Raven Form")
                .Then(Verify.Power("Raven Form").IsActive())
                .Then(Verify.Hero().TravelSpeed(2).SearchDice(2).HasUsedAction().CanGainGrace(false));
        }

        [Test]
        public void Celerity_NoNewFormSelected()
        {
            TestScenario
                .Game.WithHero("Druid").HasPowers("Celerity", "Sprite Form").At("Castle").Secrecy(0)
                .When.Player.TakesAction("Celerity")
                .When.Player.SelectsLocation("Village")
                .Then(Verify.Player.Hero("Druid").Secrecy(1))
                .Then(Verify.Hero().Location("Village").HasFreeAction().HasAvailableActions("Sprite Form", "Skip Free Action").Secrecy(1))
                .When.Player.TakesAction("Skip Free Action")
                .Then(Verify.Hero().HasUsedAction().HasFreeAction(false).Secrecy(1));
        }

        // Raven Form (Action): Deactivate all Forms. Optionally activate.
        // Active: +1 die in searches. When you travel, you may move two spaces. You cannot gain Grace.
        [Test]
        public void RavenForm_Activate()
        {
            TestScenario.Game
                .WithHero("Druid").HasPowers("Sprite Form", "Raven Form").Power("Sprite Form").IsActive()
                .When.Player.TakesAction("Raven Form")
                .Then(Verify.Hero().TravelSpeed(2).SearchDice(2).CanGainGrace(false).HasUsedAction())
                .Then(Verify.Power("Raven Form").IsActive())
                .Then(Verify.Power("Sprite Form").IsActive(false));
        }

        [Test]
        public void RavenForm_Deactivate()
        {
            TestScenario.Game
                .WithHero("Druid").HasPowers("Raven Form").Power("Raven Form").IsActive()
                .When.Player.TakesAction("Deactivate Form")
                .Then(Verify.Hero().TravelSpeed(1).SearchDice(1).HasUsedAction());
        }

        // Sprite Form (Action): Deactivate all Forms. Optionally activate.
        // Ignore blights' effects unless the Necromancer is present. You cannot gain Grace.
        [Test]
        public void SpriteForm_Deactivate()
        {
            TestScenario.Game
                .WithHero("Druid").HasPowers("Sprite Form").Power("Sprite Form").IsActive()
                .When.Player.TakesAction("Deactivate Form")
                .Then(Verify.Hero().IsIgnoringBlights(false).HasUsedAction());
        }

        [Test]
        public void SpriteForm_DoesNotIgnoreBlightsWhenNecromancerIsPresent()
        {
            TestScenario.Game
                .Necromancer.At("Ruins")
                .WithHero("Druid").HasPowers("Sprite Form").At("Ruins")
                .Given.Hero().Power("Sprite Form").IsActive()
                .Then(Verify.Hero().CanGainGrace(false).IsIgnoringBlights(false));
        }

        [Test]
        public void SpriteForm_IgnoreBlightsWhileActive()
        {
            TestScenario.Game
                .WithHero("Druid").HasPowers("Sprite Form").Power("Sprite Form").IsActive()
                .Then(Verify.Hero().CanGainGrace(false).IsIgnoringBlights());
        }

        [Test]
        public void SpriteForm_IgnoreEnemyLairs()
        {
            TestScenario.Game
                .WithHero("Druid").At("Village").HasPowers("Sprite Form").Power("Sprite Form").IsActive()
                .Location("Village").HasBlights("Skeletons")
                .When.Player.TakesAction("End Turn")
                .Then(Verify.Hero().CanGainGrace(false).IsIgnoringBlights().IsFacingEnemies());
        }

        [Test]
        public void SpriteForm_Activate()
        {
            TestScenario.Game
                .WithHero("Druid").HasPowers("Sprite Form", "Raven Form").Power("Raven Form").IsActive()
                .When.Player.TakesAction("Sprite Form")
                .Then(Verify.Hero().CanGainGrace(false).HasUsedAction())
                .Then(Verify.Power("Sprite Form").IsActive())
                .Then(Verify.Power("Raven Form").IsActive(false));
        }

        [Test]
        public void Tranquility()
        {
            TestScenario.Game
                .WithHero("Druid").HasPowers("Tranquility")
                .Then(Verify.Hero().DefaultGrace(8).Grace(5));
        }

        // Tree Form (Action): Deactivate all Forms. Optionally activate.
        // Gain 2 Grace (up to default) at the start of your turn. Your actions can only be to hide or use a Druid power.
        [Test]
        public void TreeForm_Activate()
        {
            TestScenario.Game
                .WithHero("Druid").HasPowers("Tree Form", "Wolf Form").Power("Wolf Form").IsActive()
                .When.Player.TakesAction("Tree Form")
                .Then(Verify.Hero().HasUsedAction())
                .Then(Verify.Power("Tree Form").IsActive())
                .Then(Verify.Power("Wolf Form").IsActive(false));
        }

        [Test]
        public void TreeForm_Deactivate()
        {
            TestScenario.Game
                .WithHero("Druid").NotAt("Monastery").HasPowers("Tree Form").Grace(0)
                .Power("Tree Form").IsActive()
                .When.Player.TakesAction("Deactivate Form")
                .Given.Hero().IsTakingTurn()
                .Then(Verify.Hero().Grace(0).HasAvailableActions("Hide", "Search", "Travel", "Tree Form", "End Turn"));
        }

        [Test]
        public void TreeForm_GainTwoGraceAtStartOfTurn()
        {
            TestScenario.Game
                .WithHero("Druid").HasPowers("Tree Form").Grace(0)
                .Power("Tree Form").IsActive()
                .When.Player.StartsTurn()
                .Then(Verify.Hero().Grace(2));
        }

        [Test]
        public void TreeForm_MaxAtDefaultGrace()
        {
            TestScenario.Game
                .WithHero("Druid").HasPowers("Tree Form").Grace(4)
                .Power("Tree Form").IsActive()
                .When.Player.StartsTurn()
                .Then(Verify.Hero().DefaultGrace(5).Grace(5));
        }

        [Test]
        public void TreeForm_RestrictedActions()
        {
            TestScenario.Game
                .WithHero("Druid").HasPowers("Tree Form", "Celerity", "Raven Form", "Sprite Form", "Wolf Form")
                .Secrecy(0) // secrecy needs to be <5 in order to use hide
                .Power("Tree Form").IsActive()
                .Given.Hero("Druid").IsTakingTurn()
                .Then(Verify.Hero().HasAvailableActions("Hide", "Celerity", "Raven Form", "Sprite Form", "Wolf Form", "Deactivate Form", "End Turn"));
        }

        [Test]
        public void TreeForm_And_Celerity()
        {
            TestScenario.Game
                .WithHero("Druid").At("Village").HasPowers("Celerity", "Tree Form")
                .When.Player.TakesAction("Celerity").SelectsLocation("Castle")
                .Then(Verify.Player.Hero("Druid").Commands.Exactly("Tree Form", "Skip Free Action"))
                .When.Player.TakesAction("Tree Form")
                .Then(Verify.Player.Hero("Druid").Commands.Exactly());
        }


        // Vines (Tactic): Exhaust to fight or elude with 4 dice.
        [Test]
        public void Vines_Fight()
        {
            TestScenario.Game
                .WithHero("Druid").HasPowers("Vines")
                .Given.Hero().IsFacingEnemy("Zombie")
                .When.Player.CompletesConflict("Zombie", "Vines [fight]", Fake.Rolls(1, 2, 3, 4))
                .Then(Verify.Hero().RolledNumberOfDice(4).WasWounded())
                .Then(Verify.Power("Vines").IsExhausted());
        }

        [Test]
        public void Vines_Elude()
        {
            TestScenario.Game
                .WithHero("Druid").HasPowers("Vines")
                .Given.Hero().IsFacingEnemy("Zombie")
                .When.Player.CompletesConflict("Zombie", "Vines [elude]", Fake.Rolls(1, 2, 3, 4))
                .Then(Verify.Hero().RolledNumberOfDice(4))
                .Then(Verify.Power("Vines").IsExhausted());
        }

        [Test]
        public void Visions_CannotIgnoreRenewal()
        {
            TestScenario
                .Game.WithHero("Druid").HasPowers("Visions")
                .Given.Hero().HasDrawnEvent("Renewal")
                .Then(Verify.Hero().HasUnresolvedEvents(1).CurrentEvent.CanBeIgnored(false))
                .Then(Verify.Player.EventView.HasOptions("Continue"));
        }

        [Test]
        public void Visions_IgnoreEvent()
        {
            TestScenario
                .Game.WithHero("Druid").HasPowers("Visions")
                .Given.Hero().HasDrawnEvent("Anathema")
                .Then(Verify.Player.EventView.HasOptions("Continue", "Ignore [Visions]"))
                .When.Player.SelectsEventOption("Ignore [Visions]")
                .Then(Verify.Hero().LostGrace(0)) // Anathema causes hero to lose 1 Grace unless ignored
                .Then(Verify.Power("Visions").IsExhausted());
        }

        [Test]
        public void WolfForm_Activate()
        {
            TestScenario.Game
                .WithHero("Druid").HasPowers("Wolf Form")
                .When.Player.TakesAction("Wolf Form")
                .Then(Verify.Hero().HasUsedAction().CanGainGrace(false).FightDice(2).EludeDice(2))
                .Then(Verify.Power("Wolf Form").IsActive());
        }
    }
}
﻿using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class RogueTest
    {
        // Ambush (Tactic): Spend 1 Secrecy to fight with 3 dice.
        [Test]
        public void Ambush()
        {
            TestScenario.Game
                .WithHero("Rogue").HasPowers("Ambush").IsFacingEnemy("Skeleton")
                .Then(Verify.Player.ConflictModel.HasTactics("Ambush", "Fight", "Elude"))
                .When.Player.Targets("Skeleton").UsesTactic("Ambush").ResolvesConflict()
                .Then(Verify.Player.Hero("Rogue").LostSecrecy(1))
                .Then(Verify.Player.ConflictModel.Rolled(6, 6, 6));
        }

        [Test]
        public void Ambush_RequiresSecrecy()
        {
            TestScenario.Game
                .WithHero("Rogue").Secrecy(0).HasPowers("Ambush").IsFacingEnemy("Skeleton")
                .Then(Verify.Player.ConflictModel.HasTactics("Fight", "Elude"));
        }

        // Contacts (Bonus): Exhaust at any time to gain 1 Secrecy (up to 7).
        [Test]
        public void Contacts()
        {
            TestScenario.Game
                .WithHero("Rogue").Secrecy(0).HasPowers("Contacts")
                .When.Player.TakesAction("Contacts")
                .Then(Verify.Power("Contacts").IsExhausted())
                .Then(Verify.Player.Hero("Rogue").Secrecy(1));
        }

        [Test]
        public void Contacts_NotUsuableAtMaxSecrecy()
        {
            TestScenario.Game
                .WithHero("Rogue").Secrecy(7).HasPowers("Contacts")
                .Then(Verify.Player.Hero("Rogue").Commands.Excludes("Contacts"));
        }

        // Diversion (Action): Spend 1 Secrecy to negate the effects of one blight in your location until the Necromancer ends a turn there.
        [Test]
        public void Diversion()
        {
            TestScenario.Game
                .Location("Village").HasBlights("Desecration", "Confusion")
                .WithHero("Rogue").HasPowers("Diversion").At("Village")
                .Then(Verify.Player.Hero("Rogue").Commands.Includes("Diversion"))
                .When.Player.TakesAction("Diversion")
                .Then(Verify.Player.BlightSelectionView.Location("Village")
                    .WithBlights("Desecration", "Confusion"))
                .When.Player.SelectsBlight("Village", "Desecration")
                .Then(Verify.Player.BoardView.Location("Village").Blight("Desecration").IsSupressed());
        }

        [Test]
        public void Diversion_EndsWhenNecromancerEndsTurnAtLocation()
        {
            TestScenario.Game
                .Location("Village").HasBlights("Desecration")
                .WithHero("Rogue").HasPowers("Diversion").At("Village")
                .When.Player.TakesAction("Diversion")
                .When.Player.SelectsBlight("Village", "Desecration")
                .Given.Game.Necromancer.At("Village")
                .When.Game.NecromancerActs(Fake.Rolls(6)) // stays at village
                .When.Player.AcceptsNecromancerTurn()
                .Then(Verify.Player.BoardView.Location("Village").Blight("Desecration").IsSupressed(false));
        }

        [Test]
        public void Diversion_RequiresSecrecry()
        {
            TestScenario.Game
                .Location("Village").HasBlights("Desecration")
                .WithHero("Rogue").Secrecy(0).HasPowers("Diversion").At("Village")
                .Then(Verify.Player.Hero("Rogue").Commands.Excludes("Diversion"));
        }

        [Test]
        public void Diversion_RequiresTarget()
        {
            TestScenario.Game
                .Location("Village").HasBlights()
                .WithHero("Rogue").HasPowers("Diversion").At("Village")
                .Then(Verify.Player.Hero("Rogue").Commands.Excludes("Diversion"));
        }

        [Test]
        public void Diversion_Restored()
        {
            TestScenario.Game
                .Location("Village").HasBlights("Desecration")
                .WithHero("Rogue").HasPowers("Diversion").At("Village")
                .When.Player.TakesAction("Diversion")
                .When.Player.SelectsBlight("Village", "Desecration")
                .When.Game.Saved().Restored()
                .Then(Verify.Player.BoardView.Location("Village").Blight("Desecration").IsSupressed());
        }

        // Eavesdrop (Action): Spend 1 Secrecy to search with 2 dice.
        // Sabotage (Action): Spend 1 Secrecy in the Necromancer's location to cause -1 Darkness.
        // Sap (Bonus): Exhaust during your turn to reduce the might of a blight in your location by 1 until your next turn.
        // Shadow Cloak (Bonus): +1 die when eluding.
        // Skulk (Tactic): Elude with 2 dice and add 1 to the highest die.
        // Stealth (Bonus): Any time you lose or spend Secrecy, you can spend 1 Grace instead.
        // Vanish (Tactic): Elude with 2 dice. Gain 1 Secrecy (up to 7) if you roll 2 successes.
    }
}

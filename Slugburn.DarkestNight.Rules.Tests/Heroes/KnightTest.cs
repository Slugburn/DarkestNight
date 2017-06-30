using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class KnightTest
    {
        [TestCase("Hide")]
        [TestCase("Search")]
        public void OathOfVengeance_Broken(string action)
        {
            // Hide or search; you lose 1 Grace
            TestScenario.Game
                .WithHero("Knight").HasPowers("Oath of Vengeance").Power("Oath of Vengeance").IsActive()
                .When.Player.TakesAction(action)
                .Then(Verify.Hero().HasUsedAction().LostGrace());
        }

        // Charge (Tactic): Fight with 2 dice.
        [Test]
        public void Charge()
        {
            TestScenario.Game
                .WithHero("Knight").HasPowers("Charge")
                .Given.Hero().FacesEnemy("Skeleton")
                .When.Player.CompletesConflict("Skeleton", "Charge", Fake.Rolls(1, 6))
                .Then(Verify.Hero().RolledNumberOfDice(2));
        }

        [Test]
        public void ConsecratedBlade()
        {
            TestScenario.Game
                .WithHero("Knight").HasPowers("Consecrated Blade")
                .Then(Verify.Hero().FightDice(2));
        }

        [Test]
        public void ConsecratedBlade_Exhausted()
        {
            TestScenario.Game
                .WithHero("Knight").HasPowers("Consecrated Blade")
                .Power("Consecrated Blade").IsExhausted()
                .Then(Verify.Hero().FightDice(1));
        }

        [Test]
        public void ConsecratedBlade_Suppressed()
        {
            TestScenario.Game
                .WithHero("Knight").HasPowers("Consecrated Blade").At("Village")
                .Location("Village").Blights("Corruption")
                .Then(Verify.Hero().FightDice(1));
        }

        // Hard Ride (Action): Move twice, but gain no Secrecy.
        [Test]
        public void HardRide()
        {
            TestScenario.Game
                .WithHero("Knight").HasPowers("Hard Ride").Secrecy(0)
                .When.Player.TakesAction("Hard Ride")
                .Then(Verify.Hero().HasUsedAction().AvailableMovement(2).Secrecy(0)); // No Secrecy gain
        }

        // Holy Mantle (Bonus): +1 to default Grace. Add 1 to each die when praying.
        [Test]
        public void HolyMantle()
        {
            TestScenario.Game
                .WithHero("Knight").HasPowers("Holy Mantle").Grace(0)
                .Then(Verify.Hero().DefaultGrace(6).Grace(0))
                .When.Player.TakesAction("Pray", Fake.Rolls(2, 3)).AcceptsRoll()
                .Then(Verify.Hero().Rolled(3, 4).DefaultGrace(6).Grace(2).HasUsedAction());
        }

        // Oath of Defense (Action): 
        // Active: Gain 1 Grace (up to default) at start of turn.
        // Fulfill: No blights at location; You gain 1 Grace.
        // Break: Leave location; you lose all Grace.
        [Test]
        public void OathOfDefense_ActivateAtLocationWithBlight()
        {
            TestScenario.Game
                .WithHero("Knight").HasPowers("Oath of Defense").At("Village").Grace(0)
                .Location("Village").Blights("Shades")
                .When.Player.TakesAction("Oath of Defense")
                .Then(Verify.Hero().Grace(0).HasUsedAction())
                .Then(Verify.Power("Oath of Defense").IsActive());
        }

        [Test]
        public void OathOfDefense_ActivateAtLocationWithNoBlight()
        {
            TestScenario.Game
                .WithHero("Knight").HasPowers("Oath of Defense").Grace(0)
                .When.Player.TakesAction("Oath of Defense")
                // gains grace and deactivates immediately
                .Then(Verify.Hero().Grace(1).HasUsedAction())
                .Then(Verify.Power("Oath of Defense").IsActive(false));
        }

        [Test]
        public void OathOfDefense_Active()
        {
            TestScenario
                .Game.WithHero("Knight").HasPowers("Oath of Defense").Grace(0).At("Village")
                .Given.Location("Village").Blights("Shades")
                .Given.Hero().Power("Oath of Defense").IsActive()
                .When.Player.StartsTurn()
                .Then(Verify.Hero().Grace(1).HasUnresolvedEvents(1));
        }

        [Test]
        public void OathOfDefense_Break()
        {
            TestScenario
                .Game.WithHero("Knight").HasPowers("Oath of Defense").Grace(4).At("Village")
                .Given.Location("Village").Blights("Shades")
                .Given.Hero().Power("Oath of Defense").IsActive()
                .Given.Hero().MovesTo("Mountains")
                .Then(Verify.Hero().Grace(0))
                .Then(Verify.Power("Oath of Defense").IsActive(false));
        }

        [Test]
        public void OathOfDefense_Fulfill()
        {
            TestScenario
                .Game.WithHero("Knight").HasPowers("Oath of Defense").Grace(0).At("Village")
                .Given.Location("Village").Blights("Shades")
                .Given.Hero().Power("Oath of Defense").IsActive()
                .When.Game.BlightDestroyed("Village", "Shades")
                .Then(Verify.Hero().Grace(1))
                .Then(Verify.Power("Oath of Defense").IsActive(false));
        }

        // Oath of Purging (Action): 
        // Active: +2 dice in fights when attacking blights.
        // Fulfill: Destroy a blight; you gain 1 Grace.
        // Break: Enter the Monastery; you lose 1 Grace.
        [Test]
        public void OathOfPurging_Activate()
        {
            TestScenario
                .Game.WithHero("Knight").HasPowers("Oath of Purging").NotAt("Monastery")
                .When.Player.TakesAction("Oath of Purging")
                .Then(Verify.Power("Oath of Purging").IsActive());
        }

        [Test]
        public void OathOfPurging_ActiveAndFulfill()
        {
            TestScenario.Game
                .WithHero("Knight").HasPowers("Oath of Purging").At("Village").Grace(0)
                .Power("Oath of Purging").IsActive()
                .Location("Village").Blights("Skeletons")
                .When.Player.TakesAction("Attack").Fights(Fake.Rolls(4, 5, 6))
                .Then(Verify.Hero().RolledNumberOfDice(3).Grace(1).LostSecrecy().HasUsedAction())
                .Then(Verify.Power("Oath of Purging").IsActive(false));
        }

        [Test]
        public void OathOfPurging_Break()
        {
            // Enter the Monastery; you lose 1 Grace.
            TestScenario
                .Game.WithHero("Knight").HasPowers("Oath of Purging").At("Village")
                .Given.Hero().Power("Oath of Purging").IsActive()
                .Given.Hero().MovesTo("Monastery")
                .Then(Verify.Hero().LostGrace())
                .Then(Verify.Power("Oath of Purging").IsActive(false));
        }

        // Oath of Valor (Action):
        // Active: +1 die in fights.
        // Fulfill: Win a fight; You may activate any Oath immediately.
        // Break: Attempt to elude; you lose 1 Grace.
        [Test]
        public void OathOfValor_Activate()
        {
            TestScenario.Game
                .WithHero("Knight").HasPowers("Oath of Valor")
                .When.Player.TakesAction("Oath of Valor")
                .Then(Verify.Power("Oath of Valor").IsActive());
        }

        [Test]
        public void OathOfValor_Active()
        {
            // +1 die in fights.
            const string powerName = "Oath of Valor";
            TestScenario.Game
                .WithHero("Knight").HasPowers(powerName).Power(powerName).IsActive()
                .Then(Verify.Hero().FightDice(2));
        }

        [Test]
        public void OathOfValor_Break()
        {
            // Attempt to elude; you lose 1 Grace.
            TestScenario
                .Game.WithHero("Knight").HasPowers("Oath of Valor", "Oath of Vengeance")
                .Power("Oath of Valor").IsActive()
                .Given.Hero().FacesEnemy("Skeleton")
                .When.Player.Eludes()
                .Then(Verify.Hero().LostGrace())
                .Then(Verify.Power("Oath of Valor").IsActive(false));
        }

        [Test]
        public void OathOfValor_FulfillOnAttack()
        {
            // Win a fight; You may activate any Oath immediately.
            TestScenario.Game.WithHero("Knight").HasPowers("Oath of Valor")
                .Given.Hero().Power("Oath of Valor").IsActive()
                .Given.Hero().FacesEnemy("Skeleton")
                .When.Player.Fights(Fake.Rolls(6, 6))
                .Then(Verify.Hero().HasFreeAction().HasAvailableActions("Oath of Valor", "Skip Free Action"))
                .Then(Verify.Power("Oath of Valor").IsActive(false));
        }

        [Test]
        public void OathOfValor_FulfillOnDefense()
        {
            // Win a fight; You may activate any Oath immediately.
            TestScenario
                .Game.WithHero("Knight").HasPowers("Oath of Valor", "Oath of Vengeance").At("Village")
                .Power("Oath of Valor").IsActive()
                .Given.Location("Village").Blights("Skeletons")
                .Given.Hero().FacesEnemy("Skeleton")
                .When.Player.Fights(Fake.Rolls(6, 6))
                .Then(Verify.Hero().HasFreeAction().HasAvailableActions("Oath of Valor", "Oath of Vengeance", "Skip Free Action"))
                .Then(Verify.Power("Oath of Valor").IsActive(false));
        }

        // Oath of Vengeance
        [Test]
        public void OathOfVengeance_Activate()
        {
            const string powerName = "Oath of Vengeance";
            TestScenario.Game
                .WithHero("Knight").HasPowers(powerName)
                .When.Player.TakesAction(powerName)
                .Then(Verify.Power(powerName).IsActive());
        }

        [Test]
        public void OathOfVengeance_Active()
        {
            // Add 1 to highest die when fighting the Necromancer.
            // Win fight versus the Necromancer; you get a free action.
            TestScenario.Game
                .NecromancerAt("Ruins")
                .WithHero("Knight").HasPowers("Oath of Vengeance", "Charge", "Consecrated Blade").At("Ruins")
                .Given.Hero().Power("Oath of Vengeance").IsActive()
                .When.Player.TakesAction("Attack Necromancer").CompletesConflict("Necromancer", "Charge", Fake.Rolls(2, 3, 6))
                .Then(Verify.Hero().Rolled(2, 3, 7).FightDice(2).HasFreeAction())
                .Then(Verify.Power("Oath of Vengeance").IsActive(false));
        }

        // Reckless Abandon
        // Tactic: Fight with 4 dice. Lose 1 Grace if you roll fewer than 2 successes.
        [Test]
        public void RecklessAbandon()
        {
            TestScenario
                .Game.WithHero("Knight").HasPowers("Reckless Abandon")
                .Given.Hero().FacesEnemy("Vampire")
                .When.Player.CompletesConflict("Vampire", "Reckless Abandon", Fake.Rolls(1, 2, 3, 4))
                .Then(Verify.Hero().RolledNumberOfDice(4).LostGrace());
        }

        // Sprint
        // Tactic: Elude with 2 dice
        [Test]
        public void Sprint()
        {
            TestScenario
                .Game.WithHero("Knight").HasPowers("Sprint")
                .Given.Hero().FacesEnemy("Skeleton")
                .When.Player.CompletesConflict("Skeleton", "Sprint", Fake.Rolls(4))
                .Then(Verify.Hero().RolledNumberOfDice(2));
        }
    }
}
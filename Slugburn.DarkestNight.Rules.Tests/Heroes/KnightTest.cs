using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class KnightTest
    {
        [Test]
        public void Charge()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers("Charge").Location("Village"))
                .GivenLocation("Village", x=>x.Blight("Skeletons"))
                .WhenPlayerTakesAttackAction(x => x.Tactic("Charge").Rolls(1,6))
                .ThenHero(x => x.RolledNumberOfDice(2).HasUsedAction().LostSecrecy());
        }

        [Test]
        public void ConsecratedBlade()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers("Consecrated Blade"))
                .ThenHero(x => x.FightDice(2));
        }

        [Test]
        public void ConsecratedBlade_Suppressed()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers("Consecrated Blade").Location("Village"))
                .GivenLocation("Village", x=>x.Blight("Corruption"))
                .ThenHero(x => x.FightDice(1));
        }

        [Test]
        public void ConsecratedBlade_Exhausted()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers("Consecrated Blade"))
                .GivenPower("Consecrated Blade", x=>x.IsExhausted())
                .ThenHero(x => x.FightDice(1));
        }

        [Test]
        public void HardRide()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers("Hard Ride").Secrecy(0))
                .WhenPlayerTakesAction("Hard Ride")
                .ThenHero(x => x.HasUsedAction().AvailableMovement(2).Secrecy(0)); // No Secrecy gain
        }

        [Test]
        public void HolyMantle()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers("Holy Mantle").Grace(0))
                .ThenHero(x => x.DefaultGrace(6).Grace(0))
                .WhenPlayerTakesAction("Pray", x => x.Rolls(2, 3))
                .WhenPlayerAcceptsRoll()
                .ThenHero(x => x.Rolled(3, 4).DefaultGrace(6).Grace(2));
        }

        [Test]
        public void OathOfDefense_ActivateAtLocationWithNoBlight()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers("Oath of Defense").Grace(0))
                .WhenPlayerTakesAction("Oath of Defense")
                // gains grace and deactivates immediately
                .ThenHero(x=>x.Grace(1).HasUsedAction()) 
                .ThenPower("Oath of Defense", x=>x.IsActive(false));
        }

        [Test]
        public void OathOfDefense_ActivateAtLocationWithBlight()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers("Oath of Defense").Grace(0).Location("Village"))
                .GivenLocation("Village", x=>x.Blight("Shades"))
                .WhenPlayerTakesAction("Oath of Defense")
                .ThenHero(x => x.Grace(0).HasUsedAction())
                .ThenPower("Oath of Defense", x => x.IsActive());
        }

        [Test]
        public void OathOfDefense_Active()
        {
            TestScenario
                .Given.Game(g => g.Hero("Knight", x => x.HasPowers("Oath of Defense").Grace(0).Location("Village")))
                .Given.Location("Village", x => x.Blight("Shades"))
                .Given.ActingHero(g => g.Power("Oath of Defense", x => x.IsActive()))
                .When.Hero(x => x.StartsTurn())
                .Then.Hero(x => x.Grace(1));
        }

        [Test]
        public void OathOfDefense_Fulfill()
        {
            TestScenario
                .Given.Game(g => g.Hero("Knight", x => x.HasPowers("Oath of Defense").Grace(0).Location("Village")))
                .Given.Location("Village", x => x.Blight("Shades"))
                .Given.ActingHero(h => h.Power("Oath of Defense", x => x.IsActive()))
                .When.Game(x => x.BlightDestroyed("Village", "Shades"))
                .Then.Hero(x => x.Grace(1).Power("Oath of Defense", p => p.IsActive(false)));
        }

        [Test]
        public void OathOfDefense_Break()
        {
            TestScenario
                .Given.Game(g=>g.Hero("Knight", x => x.HasPowers("Oath of Defense").Grace(4).Location("Village")))
                .Given.Location("Village", x => x.Blight("Shades"))
                .Given.ActingHero(h=>h.Power("Oath of Defense", x => x.IsActive()))
                .When.Hero(x=>x.MovesTo("Mountains"))
                .Then.Hero(h => h
                .Grace(0)
                .Power("Oath of Defense", x => x.IsActive(false)));
        }

        [Test]
        public void OathOfPurging_Activate()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers("Oath of Purging").Location("Village"))
                .GivenLocation("Village", x=>x.Blight("Skeletons"))
                .WhenPlayerTakesAction("Oath of Purging")
                .ThenPower("Oath of Purging", x => x.IsActive());
        }

        [Test]
        public void OathOfPurging_ActiveAndFulfill()
        {
            // +2 dice in fights when attacking blights.
            // Destroy a blight; you gain 1 Grace.
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers("Oath of Purging").Location("Village").Grace(0))
                .GivenLocation("Village", x=>x.Blight("Skeletons"))
                .GivenPower("Oath of Purging", x=>x.IsActive())
                .WhenPlayerTakesAttackAction()
                .ThenHero(x => x.Grace(1).HasUsedAction().LostSecrecy().RolledNumberOfDice(3))
                .ThenPower("Oath of Purging", x=>x.IsActive(false));
        }

        [Test]
        public void OathOfPurging_Break()
        {
            // Enter the Monastery; you lose 1 Grace.
            TestScenario
                .Given.Game(g => g.Hero("Knight", x => x.HasPowers("Oath of Purging").Location("Village")))
                .Given.ActingHero(h => h.Power("Oath of Purging", x => x.IsActive()))
                .When.Hero(x => x.MovesTo("Monastery"))
                .Then.Hero(h => h
                    .LostGrace()
                    .Power("Oath of Purging", x => x.IsActive(false)));
        }

        // Oath of Valor
        [Test]
        public void OathOfValor_Activate()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers("Oath of Valor").Location("Village"))
                .GivenLocation("Village", x => x.Blight("Skeletons"))
                .WhenPlayerTakesAction("Oath of Valor")
                .ThenPower("Oath of Valor", x => x.IsActive());
        }

        [Test]
        public void OathOfValor_Active()
        {
            // +1 die in fights.
            const string powerName = "Oath of Valor";
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers(powerName))
                .GivenPower(powerName, x => x.IsActive())
                .ThenHero(x=>x.FightDice(2));
        }

        [Test]
        public void OathOfValor_FulfillOnAttack()
        {
            // Win a fight; You may activate any Oath immediately.
            const string powerName = "Oath of Valor";
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers(powerName).Location("Village"))
                .GivenLocation("Village", x=>x.Blight("Skeletons"))
                .GivenPower(powerName, x => x.IsActive())
                .WhenPlayerTakesAttackAction(x=>x.Rolls(6,6))
                .ThenHero(x=>x.HasUsedAction().LostSecrecy().HasAvailableActions(powerName))
                .ThenPower(powerName, x=>x.IsActive(false));
        }

        [Test]
        public void OathOfValor_FulfillOnDefense()
        {
            // Win a fight; You may activate any Oath immediately.
            TestScenario
                .Given.Game(g => g.Hero("Knight", x => x.HasPowers("Oath of Valor", "Oath of Vengeance").Location("Village")))
                .Given.Location("Village", x => x.Blight("Skeletons"))
                .Given.ActingHero(h => h.Power("Oath of Valor", x => x.IsActive()))
                .When.Hero(h => h.Fights(x => x.Rolls(6, 6)))
                .Then.Hero(h => h
                    .HasAvailableActions("Oath of Valor", "Oath of Vengeance")
                    .Power("Oath of Valor", x => x.IsActive(false)));

        }

        [Test]
        public void OathOfValor_Break()
        {
            // Attempt to elude; you lose 1 Grace.
            TestScenario
                .Given.Game(g => g.Hero("Knight", x => x.HasPowers("Oath of Valor", "Oath of Vengeance").Location("Village")))
                .Given.Location("Village", x => x.Blight("Skeletons"))
                .Given.ActingHero(h => h.Power("Oath of Valor", x => x.IsActive()))
                .When.Hero(h => h.Eludes(x => x.Rolls(6)))
                .Then.Hero(h => h
                    .LostGrace()
                    .Power("Oath of Valor", p => p.IsActive(false)));
        }

        // Oath of Vengeance
        [Test]
        public void OathOfVengeance_Activate()
        {
            const string powerName = "Oath of Vengeance";
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers(powerName))
                .WhenPlayerTakesAction(powerName)
                .ThenPower(powerName, x => x.IsActive());
        }

        [Test]
        public void OathOfVengeance_Active()
        {
            // Add 1 to highest die when fighting the Necromancer.
            // Win fight versus the Necromancer; you get a free action.
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers("Oath of Vengeance", "Charge", "Consecrated Blade").Location("Ruins"))
                .GivenNecromancerLocation("Ruins")
                .GivenPower("Oath of Vengeance", x=>x.IsActive())
                .WhenPlayerTakesAttackAction(x=>x.Action("Fight Necromancer").Tactic("Charge").Rolls(2, 3, 6))
                .ThenHero(x=>x.Rolled(2, 3, 7).FightDice(2).HasUsedAction().HasFreeAction())
                .ThenPower("Oath of Vengeance", x=>x.IsActive(false));
        }

        [TestCase("Hide")]
        [TestCase("Search")]
        public void OathOfVengeance_Broken(string action)
        {
            // Hide or search; you lose 1 Grace
            new TestScenario()
                .GivenHero("Knight", x => x.HasPowers("Oath of Vengeance"))
                .GivenPower("Oath of Vengeance", x => x.IsActive())
                .WhenPlayerTakesAction(action)
                .ThenHero(x=>x.HasUsedAction().LostGrace());
        }

        // Reckless Abandon
        // Tactic: Fight with 4 dice. Lose 1 Grace if you roll fewer than 2 successes.
        [Test]
        public void RecklessAbandon()
        {
            TestScenario
                .Given.Game(g=>g.Hero("Knight", x => x.HasPowers("Reckless Abandon").Location("Village")))
                .Given.Location("Village", x => x.Blight("Vampire"))
                .When.Hero(h => h.Fights(x => x.Tactic("Reckless Abandon").Rolls(1, 2, 3, 4)))
                .Then.Hero(x => x.RolledNumberOfDice(4).LostGrace());
        }

        // Sprint
        // Tactic: Elude with 2 dice
        [Test]
        public void Sprint()
        {
            TestScenario
                .Given.Game(g => g.Hero("Knight", x => x.HasPowers("Sprint").Location("Village")))
                .Given.Location("Village", x => x.Blight("Skeletons"))
                .When.Hero(h => h.Eludes(x => x.Tactic("Sprint")))
                .Then.Hero(x => x.RolledNumberOfDice(2));
        }
    }
}

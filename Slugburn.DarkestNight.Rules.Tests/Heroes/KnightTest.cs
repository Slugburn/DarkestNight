using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class KnightTest
    {
        [Test]
        public void Charge()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Charge").Location(Location.Village))
                .GivenSpace(Location.Village, x=>x.Blight(Blight.Skeletons))
                .WhenPlayerTakesAttackAction(x => x.Tactic("Charge").Rolls(1,6))
                .ThenPlayer(x => x.RolledNumberOfDice(2));
        }

        [Test]
        public void ConsecratedBlade()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Consecrated Blade"))
                .ThenHero(x => x.FightDice(2));
        }

        [Test]
        public void ConsecratedBlade_Suppressed()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Consecrated Blade").Location(Location.Village))
                .GivenSpace(Location.Village, x=>x.Blight(Blight.Corruption))
                .ThenHero(x => x.FightDice(1));
        }

        [Test]
        public void ConsecratedBlade_Exhausted()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Consecrated Blade"))
                .GivenPower("Consecrated Blade", x=>x.IsExhausted())
                .ThenHero(x => x.FightDice(1));
        }

        [Test]
        public void HardRide()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Hard Ride").Secrecy(0))
                .WhenPlayerTakesAction("Hard Ride")
                .ThenHero(x => x.HasUsedAction().AvailableMovement(2).Secrecy(0)); // No Secrecy gain
        }

        [Test]
        public void HolyMantle()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Holy Mantle").Grace(0))
                .ThenHero(x => x.DefaultGrace(6).Grace(0))
                .WhenPlayerTakesAction("Pray", x => x.Rolls(2, 3))
                .ThenHero(x => x.Rolled(3, 4).DefaultGrace(6).Grace(2));
        }

        [Test]
        public void OathOfDefense_ActivateAtLocationWithNoBlight()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Oath of Defense").Grace(0))
                .WhenPlayerTakesAction("Oath of Defense")
                // gains grace and deactivates immediately
                .ThenHero(x=>x.Grace(1).HasUsedAction()) 
                .ThenPower("Oath of Defense", x=>x.IsActive(false));
        }

        [Test]
        public void OathOfDefense_ActivateAtLocationWithBlight()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Oath of Defense").Grace(0).Location(Location.Village))
                .GivenSpace(Location.Village, x=>x.Blight(Blight.Shades))
                .WhenPlayerTakesAction("Oath of Defense")
                .ThenHero(x => x.Grace(0).HasUsedAction())
                .ThenPower("Oath of Defense", x => x.IsActive());
        }

        [Test]
        public void OathOfDefense_Active()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Oath of Defense").Grace(0).Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Shades))
                .GivenPower("Oath of Defense", x => x.IsActive())
                .WhenHeroStartsTurn("Knight")
                .ThenHero(x => x.Grace(1));
        }

        [Test]
        public void OathOfDefense_Fulfill()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Oath of Defense").Grace(0).Location(Location.Village))
                .GivenSpace(Location.Village, x=>x.Blight(Blight.Shades))
                .GivenPower("Oath of Defense", x=>x.IsActive())
                .WhenBlightIsDestroyed(Location.Village, Blight.Shades)
                .ThenHero(x => x.Grace(1))
                .ThenPower("Oath of Defense", x => x.IsActive(false));
        }

        [Test]
        public void OathOfDefense_Break()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Oath of Defense").Grace(4).Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Shades))
                .GivenPower("Oath of Defense", x => x.IsActive())
                .WhenHeroMovesTo(Location.Mountains)
                .ThenHero(x => x.Grace(0))
                .ThenPower("Oath of Defense", x => x.IsActive(false));
        }

        [Test]
        public void OathOfPurging_Activate()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Oath of Purging").Location(Location.Village))
                .GivenSpace(Location.Village, x=>x.Blight(Blight.Skeletons))
                .WhenPlayerTakesAction("Oath of Purging")
                .ThenPower("Oath of Purging", x => x.IsActive());
        }

        [Test]
        public void OathOfPurging_ActiveAndFulfill()
        {
            // +2 dice in fights when attacking blights.
            // Destroy a blight; you gain 1 Grace.
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Oath of Purging").Location(Location.Village).Grace(0))
                .GivenSpace(Location.Village, x=>x.Blight(Blight.Skeletons))
                .GivenPower("Oath of Purging", x=>x.IsActive())
                .WhenPlayerTakesAttackAction()
                .ThenHero(x => x.Grace(1).HasUsedAction().LostSecrecy().RolledNumberOfDice(3))
                .ThenPower("Oath of Purging", x=>x.IsActive(false));
        }

        [Test]
        public void OathOfPurging_Break()
        {
            // Enter the Monastery; you lose 1 Grace.
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Oath of Purging").Location(Location.Village))
                .GivenPower("Oath of Purging", x => x.IsActive())
                .WhenHeroMovesTo(Location.Monastery)
                .ThenHero(x => x.LostGrace())
                .ThenPower("Oath of Purging", x => x.IsActive(false));
        }

        // Oath of Valor
        [Test]
        public void OathOfValor_Activate()
        {
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Oath of Valor").Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Skeletons))
                .WhenPlayerTakesAction("Oath of Valor")
                .ThenPower("Oath of Valor", x => x.IsActive());
        }

        [Test]
        public void OathOfValor_Active()
        {
            // +1 die in fights.
            const string powerName = "Oath of Valor";
            new TestScenario()
                .GivenHero("Knight", x => x.Power(powerName))
                .GivenPower(powerName, x => x.IsActive())
                .ThenHero(x=>x.FightDice(2));
        }

        [Test]
        public void OathOfValor_FulfillOnAttack()
        {
            // Win a fight; You may activate any Oath immediately.
            const string powerName = "Oath of Valor";
            new TestScenario()
                .GivenHero("Knight", x => x.Power(powerName).Location(Location.Village))
                .GivenSpace(Location.Village, x=>x.Blight(Blight.Skeletons))
                .GivenPower(powerName, x => x.IsActive())
                .WhenPlayerTakesAttackAction(x=>x.Rolls(6,6))
                .ThenHero(x=>x.HasUsedAction().LostSecrecy())
                .ThenPower(powerName, x=>x.IsActive(false))
                .ThenAvailableActions(powerName);
        }

        [Test]
        public void OathOfValor_FulfillOnDefense()
        {
            // Win a fight; You may activate any Oath immediately.
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Oath of Valor", "Oath of Vengeance").Location(Location.Village))
                .GivenSpace(Location.Village, x=>x.Blight(Blight.Skeletons))
                .GivenPower("Oath of Valor", x => x.IsActive())
                .WhenHeroFights(x=>x.Rolls(6,6))
                .ThenPower("Oath of Valor", x => x.IsActive(false))
                .ThenAvailableActions("Oath of Valor", "Oath of Vengeance");
        }

        [Test]
        public void OathOfValor_Break()
        {
            // Attempt to elude; you lose 1 Grace.
            new TestScenario()
                .GivenHero("Knight", x => x.Power("Oath of Valor", "Oath of Vengeance").Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Skeletons))
                .GivenPower("Oath of Valor", x => x.IsActive())
                .WhenHeroEludes(x => x.Rolls(6))
                .ThenHero(x=>x.LostGrace())
                .ThenPower("Oath of Valor", x => x.IsActive(false));
        }

        // Oath of Vengeance

        // Reckless Abandon

        // Sprint
    }
}

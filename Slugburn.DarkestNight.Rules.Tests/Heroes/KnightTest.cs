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
                .ThenHero(x=>x.Grace(1).HasUsedAction()) // gains grace and deactivates immediately
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
    }
}

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
                .GivenHero("Druid", x => x.Power("Animal Companion").Location(Location.Village).Secrecy(6))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Corruption))
                .WhenPlayerTakesAttackAction(x => x.Tactic("Animal Companion").Rolls(roll))
                .ThenSpace(Location.Village, x => x.Blights(expectedBlights))
                .ThenHero("Druid", x => x.HasUsedAction().Grace(5).Secrecy(5))
                .ThenPower("Animal Companion", x=>x.IsExhausted(!attackSucceeds));
        }
    }
}

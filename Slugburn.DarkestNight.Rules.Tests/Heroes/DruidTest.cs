﻿using NUnit.Framework;
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
                .GivenHero("Druid", x => x.Power("Animal Companion").Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Corruption))
                .WhenPlayerTakesAttackAction(x => x.Tactic("Animal Companion").Rolls(roll))
                .ThenSpace(Location.Village, x => x.Blights(expectedBlights))
                .ThenPlayer(x => x.RolledNumberOfDice(2))
                .ThenHero(x => x.HasUsedAction().LostSecrecy())
                .ThenPower("Animal Companion", x=>x.IsExhausted(!attackSucceeds));
        }

        [Test]
        public void Camouflage()
        {
            new TestScenario()
                .GivenHero("Druid", x => x.Power("Camouflage").Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Skeletons))
                .WhenPlayerEludes(x => x.Tactic("Camouflage").Rolls(1, 6))
                .ThenPlayer(x=>x.RolledNumberOfDice(2))
                .ThenHero(x=>x.LostGrace(0));
        }
    }
}

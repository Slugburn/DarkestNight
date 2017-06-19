using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}

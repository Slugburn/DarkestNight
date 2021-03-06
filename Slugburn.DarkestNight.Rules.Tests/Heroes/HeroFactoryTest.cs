using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class HeroFactoryTest
    {
        [TestCase("Acolyte")]
        [TestCase("Druid")]
        [TestCase("Knight")]
        [TestCase("Priest")]
        [TestCase("Prince")]
        [TestCase("Rogue")]
        [TestCase("Scholar")]
        [TestCase("Seer")]
        [TestCase("Wizard")]
        public void HasFullComplementOfPowers(string name)
        {
            var hero = HeroFactory.Create(name);
            Assert.That(hero.Name, Is.EqualTo(name));
            var powers = hero.PowerDeck.Select(PowerFactory.Create).ToList();
            Assert.That(powers.Count, Is.EqualTo(10));
            Assert.That(powers.Count(x => x.StartingPower), Is.EqualTo(4), powers.Where(x => x.StartingPower).Select(x => x.Name).ToCsv());
            Assert.That(powers.All(x => x.Name != null));
            Assert.That(powers.All(x => x.Text != null));
        }
    }
}
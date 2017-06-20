using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Heroes;

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
            var powers = hero.PowerDeck.ToList();
            Assert.That(powers.Count, Is.EqualTo(10));
            Assert.That(powers.Count(x => x.StartingPower), Is.EqualTo(4), string.Join(",", powers.Where(x => x.StartingPower).Select(x => x.Name)));
            Assert.That(powers.All(x => x.Name != null));
            Assert.That(powers.All(x => x.Text != null));
        }
    }
}

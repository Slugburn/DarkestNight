using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests
{
    [TestFixture]
    public class HeroTest
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
            var type = typeof(Hero).Assembly.GetType($"Slugburn.DarkestNight.Rules.Heroes.Impl.{name}");
            var hero = (Hero)Activator.CreateInstance(type);
            var powers = hero.PowerDeck.ToList();
            Assert.That(powers.Count, Is.EqualTo(10));
            Assert.That(powers.Count(x => x.StartingPower), Is.EqualTo(4), string.Join(",", powers.Where(x => x.StartingPower).Select(x => x.Name)));
            Assert.That(powers.All(x => x.Name != null));
            Assert.That(powers.All(x => x.Text != null));
        }
    }
}

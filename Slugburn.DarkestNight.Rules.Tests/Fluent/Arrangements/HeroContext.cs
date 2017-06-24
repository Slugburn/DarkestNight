using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class HeroContext
    {
        private readonly Hero _hero;

        public HeroContext(Hero hero)
        {
            _hero = hero;
        }

        public HeroContext HasPowers(params string[] names)
        {
            foreach (var name in names)
                _hero.LearnPower(name);
            return this;
        }

        public HeroContext At(string location)
        {
            _hero.Location = location.ToEnum<Location>();
            return this;
        }

        public HeroContext Secrecy(int value)
        {
            _hero.Secrecy = value;
            return this;
        }

        public HeroContext Grace(int value)
        {
            _hero.Grace = value;
            return this;
        }

        public HeroContext PowerDeck(params string[] powers)
        {
            _hero.PowerDeck.Clear();
            _hero.PowerDeck.AddRange(powers.Select(PowerFactory.Create));
            return this;
        }

        public HeroContext Power(string powerName, Action<PowerContext> action)
        {
            var power = _hero.GetPower(powerName);
            var context = new PowerContext(_hero, power);
            action(context);
            return this;
        }
    }
}
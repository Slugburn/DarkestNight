using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public class HeroContext
    {
        private readonly Hero _hero;

        public HeroContext(Hero hero)
        {
            _hero = hero;
        }

        public HeroContext Power(params string[] names)
        {
            foreach (var name in names)
                _hero.LearnPower(name);
            return this;
        }

        public HeroContext Location(Location location)
        {
            _hero.Location = location;
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
    }
}
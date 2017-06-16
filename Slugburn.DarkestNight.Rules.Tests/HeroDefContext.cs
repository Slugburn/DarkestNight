using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class HeroDefContext
    {
        private readonly Hero _hero;

        public HeroDefContext(Hero hero)
        {
            _hero = hero;
        }

        public HeroDefContext Power(params string[] names)
        {
            foreach (var name in names)
                _hero.LearnPower(name);
            return this;
        }

        public HeroDefContext Location(Location location)
        {
            _hero.Location = location;
            return this;
        }

        public HeroDefContext Secrecy(int value)
        {
            _hero.Secrecy = value;
            return this;
        }

        public HeroDefContext Grace(int value)
        {
            _hero.Grace = value;
            return this;
        }
    }
}
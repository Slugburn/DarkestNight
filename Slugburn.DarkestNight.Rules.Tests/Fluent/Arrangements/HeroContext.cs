using System.Linq;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class HeroContext : GameContext, IHeroContext
    {
        private readonly Hero _hero;

        public HeroContext(Game game, FakePlayer player, Hero hero) : base(game, player)
        {
            _hero = hero;
        }

        public Hero GetHero() => _hero;

        public IHeroContext HasPowers(params string[] names)
        {
            foreach (var name in names)
                _hero.LearnPower(name);
            return this;
        }

        public IHeroContext At(string location)
        {
            _hero.Location = location.ToEnum<Location>();
            return this;
        }

        public IHeroContext Secrecy(int value)
        {
            _hero.Secrecy = value;
            return this;
        }

        public IHeroContext Grace(int value)
        {
            _hero.Grace = value;
            return this;
        }

        public IHeroContext PowerDeck(params string[] powers)
        {
            _hero.PowerDeck.Clear();
            _hero.PowerDeck.AddRange(powers.Select(PowerFactory.Create));
            return this;
        }

        public IPowerContext Power(string powerName)
        {
            var power = _hero.GetPower(powerName);
            return new PowerContext(GetGame(), GetPlayer(), _hero, power );
        }
    }
}
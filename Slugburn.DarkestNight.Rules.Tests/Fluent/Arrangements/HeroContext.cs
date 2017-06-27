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

        public IHeroContext NotAt(string location)
        {
            var excluded = location.ToEnum<Location>();
            var randomLocation = Rules.Game.GetAllLocations().Except(new[] {excluded}).Shuffle().First();
            _hero.Location = randomLocation;
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

        public IHeroContext HasDrawnEvent(string eventName = null)
        {
            if (eventName != null)
            {
                // move event to first
                _hero.Game.Events.Remove(eventName);
                _hero.Game.Events.Insert(0, eventName);
            }
            if (_hero.Location == Rules.Location.Monastery)
                _hero.Location = Rules.Location.Village;
            _hero.DrawEvent();
            return this;
        }

        public IHeroContext FacesEnemy(string enemyName)
        {
            _hero.FaceEnemy(enemyName);
            return this;
        }

        public IHeroContext RefreshesPower(string powerName)
        {
            var power = _hero.GetPower(powerName);
            power.Refresh();
            return this;
        }

        public IHeroContext MovesTo(string location)
        {
            _hero.MoveTo(location.ToEnum<Location>());
            return this;
        }

    }
}
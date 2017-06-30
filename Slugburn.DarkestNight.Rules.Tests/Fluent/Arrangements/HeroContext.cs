using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Items;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class HeroContext : GameContext, IHeroContext
    {
        private readonly Hero _hero;

        public HeroContext(Game game, FakePlayer player, Hero hero) : base(game, player)
        {
            if (hero==null)
                throw new ArgumentNullException(nameof(hero));
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
            _hero.SetLocation(location.ToEnum<Location>());
            return this;
        }

        public IHeroContext NotAt(string location)
        {
            var excluded = new[]
            {
                location.ToEnum<Location>(),
                GetGame().Necromancer.Location  // avoid the Necromancer's location too
            };
            var randomLocation = Rules.Game.GetAllLocations().Except(excluded).Shuffle().First();
            _hero.MoveTo(randomLocation); 
            return this;
        }

        public IHeroContext Secrecy(int value)
        {
            _hero.Secrecy = value;
            _hero.UpdateAvailableActions();
            return this;
        }

        public IHeroContext Grace(int value)
        {
            _hero.Grace = value;
            _hero.UpdateAvailableActions();
            return this;
        }

        public IHeroContext LostGrace(int amount)
        {
            _hero.LoseGrace(amount);
            return this;
        }

        public IHeroContext LostSecrecy(int amount)
        {
            _hero.LoseSecrecy(amount);
            return this;
        }

        public IHeroContext DefaultGrace(int defaultGrace)
        {
            _hero.DefaultGrace = defaultGrace;
            return this;
        }

        public IHeroContext PowerDeck(params string[] powers)
        {
            _hero.PowerDeck.Clear();
            _hero.PowerDeck.AddRange(powers);
            return this;
        }

        public IPowerContext Power(string powerName)
        {
            var power = _hero.GetPower(powerName);
            return new PowerContext(GetGame(), GetPlayer(), _hero, power );
        }

        public IHeroContext HasDrawnEvent(string eventName = null)
        {
            _hero.Game.ActingHero = _hero;
            if (eventName != null)
            {
                // move event to first
                _hero.Game.Events.Remove(eventName);
                _hero.Game.Events.Insert(0, eventName);
            }
            if (_hero.Location == Rules.Location.Monastery)
                _hero.MoveTo(Rules.Location.Village);
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

        public IHeroContext IsTakingTurn(bool isTakingTurn = true)
        {
            var game = GetGame();
            if (isTakingTurn)
            {
                game.ActingHero = _hero;
                _hero.IsActionAvailable = true;
            }
            else
            {
                if (game.ActingHero == _hero)
                    game.ActingHero = null;
            }
            _hero.UpdateAvailableActions();
            return this;
        }

        public IHeroContext HasTakenTurn(bool hasTakenTurn = true)
        {
            _hero.HasTakenTurn = hasTakenTurn;
            _hero.UpdateAvailableActions();
            return this;
        }

        public IHeroContext HasUsedAction(bool hasTakenaction = true)
        {
            _hero.IsActionAvailable = false;
            _hero.UpdateAvailableActions();
            return this;
        }

        public IHeroContext HasItems(params string[] itemNames)
        {
            var items = itemNames.Select(ItemFactory.Create);
            foreach (var item in items)
                _hero.AddToInventory(item);
            return this;
        }

        public IHeroContext NextPowerDraws(params string[] powerNames)
        {
            var powers = (from existing in  _hero.PowerDeck
                         join name in powerNames on existing equals name
                         select existing).ToList();
            foreach (var power in powers)
            {
                _hero.PowerDeck.Remove(power);
                _hero.PowerDeck.Insert(0, power);
            }
            return this;
        }
    }
}
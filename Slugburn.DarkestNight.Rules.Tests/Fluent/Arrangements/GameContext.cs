using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Maps;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class GameContext : Given, IGameContext
    {
        public GameContext(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IGameContext Hero(string name, Action<HeroContext> def = null)
        {
            AddHero(HeroFactory.Create(name), def);
            return this;
        }

        public IGameContext Hero(Action<HeroContext> def = null)
        {
            AddHero(GenericHeroFactory.Create(), def);
            return this;
        }

        private void AddHero(Hero hero, Action<HeroContext> def)
        {
            _game.AddHero(hero, _player);
            var ctx = new HeroContext(hero);
            def?.Invoke(ctx);
            _game.ActingHero = hero;
            _player.ActiveHero = hero.Name;
        }

        public IGameContext NecromancerLocation(string location)
        {
            _game.Necromancer.Location = location.ToEnum<Location>();
            return this;
        }

        public IGameContext NextBlight(string blightName)
        {
            var blight = blightName.ToEnum<Blight>();
            _game.Maps.Insert(0, new Map(Enumerable.Repeat(blight, 7).ToArray(), new Find[6]));
            return this;
        }

        public IGameContext Darkness(int value)
        {
            _game.Darkness = value;
            return this;
        }

        public IGameContext DrawEvents(int count)
        {
            var drawnCards = _game.Events.Except(new[] {"Renewal"}).Take(count).ToList();
            drawnCards.ForEach(c => _game.Events.Remove(c));
            return this;
        }

        public IGameContext NextSearchResult(Find result)
        {
            _game.Maps.Insert(0, new Map(new Blight[7], Enumerable.Repeat(result, 6).ToArray()));
            return this;
        }
    }
}
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
    public class GameContext
    {
        private readonly Game _game;
        private readonly FakePlayer _player;

        public GameContext(Game game, FakePlayer _player)
        {
            _game = game;
            this._player = _player;
        }

        public GameContext Hero(string name, Action<HeroContext> def = null)
        {
            AddHero(HeroFactory.Create(name), def);
            return this;
        }

        public GameContext Hero(Action<HeroContext> def = null)
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

        public GameContext NecromancerLocation(string location)
        {
            _game.Necromancer.Location = location.ToEnum<Location>();
            return this;
        }

        public GameContext NextBlight(string blightName)
        {
            var blight = blightName.ToEnum<Blight>();
            _game.Maps.Insert(0, new Map(Enumerable.Repeat(blight, 7).ToArray(), new Find[6]));
            return this;
        }

        public GameContext Darkness(int value)
        {
            _game.Darkness = value;
            return this;
        }

        public GameContext DrawEvents(int count)
        {
            var drawnCards = _game.Events.Except(new[] { "Renewal" }).Take(count).ToList();
            drawnCards.ForEach(c => _game.Events.Remove(c));
            return this;
        }

        public GameContext NextSearchResult(Find result)
        {
            _game.Maps.Insert(0, new Map(new Blight[7], Enumerable.Repeat(result, 6).ToArray()));
            return this;
        }

    }
}
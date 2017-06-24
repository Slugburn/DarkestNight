using System;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class Given : IGiven
    {
        private readonly Game _game;
        private readonly FakePlayer _player;

        public Given(Game game, FakePlayer player)
        {
            _game = game;
            _player = player;
        }

        IGiven IGiven.Given => new Given(_game, _player);
        public IWhen When => new When(_game, _player);
        public IThen Then => new Then(_game, _player);

        public IGiven Game(Action<GameContext> def)
        {
            var ctx = new GameContext(_game, _player);
            def(ctx);
            return this;
        }

        public IGiven Location(string location, Action<LocationContext> def)
        {
            var space = _game.Board[location.ToEnum<Location>()];
            var context = new LocationContext(space);
            def(context);
            return this;
        }

        public IGiven ActingHero(Action<HeroContext> def)
        {
            var hero = _game.ActingHero;
            var ctx = new HeroContext(hero);
            def(ctx);
            return this;
        }

        public IGiven Configure(Func<IGiven, IGiven> setConditions)
        {
            return setConditions(this);
        }
    }
}

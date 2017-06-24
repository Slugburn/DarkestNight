using System;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class Given : TestRoot, IGiven
    {
        public Given(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IGameContext Game => new GameContext(_game, _player);

        public ILocationContext Location(string location)
        {
            var space = _game.Board[location.ToEnum<Location>()];
            return new LocationContext(_game, _player, space);
        }

        public IGiven ActingHero(Action<HeroContext> def)
        {
            var hero = _game.ActingHero;
            var ctx = new HeroContext(hero);
            def(ctx);
            return this;
        }
    }
}
using System;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class Given : TestRoot, IGiven
    {

        public Given(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IGameContext Game => new GameContext(_game, _player);

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
    }
}

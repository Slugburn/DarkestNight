using System;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public class When : IWhen
    {
        private readonly Game _game;
        private readonly FakePlayer _player;

        public When(Game game, FakePlayer player)
        {
            _game = game;
            _player = player;
        }

        IGiven IWhen.Given => new Given(_game, _player);
        IWhen IWhen.When => new When(_game, _player);
        public IThen Then => new Then(_game, _player);

        public IWhen Hero(Action<HeroActionContext> action)
        {
            var context = new HeroActionContext(_game.ActingHero);
            action(context);
            return this;
        }

        public IWhen Player(Action<PlayerActionContext> action)
        {
            var context = new PlayerActionContext(_player);
            action(context);
            return this;
        }

        public IWhen Game(Action<GameActionContext> action)
        {
            var context = new GameActionContext(_game);
            action(context);
            return this;
        }
    }
}

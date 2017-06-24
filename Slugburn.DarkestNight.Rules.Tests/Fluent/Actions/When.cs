using System;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public class When : TestRoot, IWhen
    {
        public When(Game game, FakePlayer player) :base(game,player)
        {
        }

        public IHeroActionContext Hero => new HeroActionContext(_game, _player);


        public IPlayerActionContext Player
        {
            get { return new PlayerActionContext(_game, _player); }
        }

        public IWhen Game(Action<GameActionContext> action)
        {
            var context = new GameActionContext(_game);
            action(context);
            return this;
        }
    }
}

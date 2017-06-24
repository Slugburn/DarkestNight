using System;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public class When : TestRoot, IWhen
    {
        public When(Game game, FakePlayer player) :base(game,player)
        {
        }

        public IWhen Hero(Action<IHeroActionContext> action)
        {
            var context = new HeroActionContext(_game.ActingHero);
            action(context);
            return this;
        }

        public IPlayerActionContext Player()
        {
            return new PlayerActionContext(_game,_player);
        }

        public IWhen Game(Action<GameActionContext> action)
        {
            var context = new GameActionContext(_game);
            action(context);
            return this;
        }
    }
}

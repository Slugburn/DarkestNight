using System;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public class WhenContext : TestRoot, IWhen
    {
        public WhenContext(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IHeroActionContext Hero => new HeroActionContext(base.GetGame(), base.GetPlayer());


        IPlayerActionContext IWhen.Player => new PlayerActionContext(base.GetGame(), base.GetPlayer());

        IWhen IWhen.Game(Action<GameActionContext> action)
        {
            var context = new GameActionContext(base.GetGame());
            action(context);
            return this;
        }
    }
}
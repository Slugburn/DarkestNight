using System;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public class WhenContext : TestRoot, IWhen
    {
        public WhenContext(Game game, FakePlayer player) : base(game, player)
        {
        }


        public IPlayerActionContext Player => new PlayerActionContext(GetGame(), GetPlayer());

        public IGameActionContext Game => new GameActionContext(GetGame(), GetPlayer());
    }
}
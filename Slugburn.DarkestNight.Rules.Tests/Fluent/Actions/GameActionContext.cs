using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public interface IGameActionContext : IWhen
    {
        IGameActionContext BlightDestroyed(string location, string blight);
        IGameActionContext NecromancerActs(IFakeRollContext rolls);
    }

    public class GameActionContext : WhenContext, IGameActionContext
    {
        public GameActionContext(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IGameActionContext BlightDestroyed(string location, string blight)
        {
            GetGame().DestroyBlight(location.ToEnum<Location>(), blight.ToEnum<Blight>());
            return this;
        }

        public IGameActionContext NecromancerActs(IFakeRollContext rolls = null)
        {
            GetGame().Necromancer.StartTurn();
            return this;
        }
    }
}
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public interface IGameActionContext : IWhen
    {
        IGameActionContext BlightDestroyed(string location, string blight);
        IGameActionContext NecromancerActs(IFakeContext rolls);
    }

    public class GameActionContext : WhenContext, IGameActionContext
    {
        public GameActionContext(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IGameActionContext BlightDestroyed(string location, string blightType)
        {
            var blightId = GetBlightId(location, blightType);
            GetGame().DestroyBlight(blightId);
            return this;
        }

        public IGameActionContext NecromancerActs(IFakeContext rolls = null)
        {
            GetGame().Necromancer.StartTurn();
            return this;
        }
    }
}
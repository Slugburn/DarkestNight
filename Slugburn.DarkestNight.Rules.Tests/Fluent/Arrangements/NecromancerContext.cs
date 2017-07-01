using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public interface INecromancerContext : IGameContext
    {
        INecromancerContext At(string location);
        INecromancerContext IsTakingTurn(bool isTakingTurn = true);
    }

    class NecromancerContext : GameContext, INecromancerContext
    {
        public NecromancerContext(Game game, FakePlayer player) : base(game, player)
        {
        }

        public INecromancerContext At(string location)
        {
            GetGame().Necromancer.Location = location.ToEnum<Location>();
            return this;
        }

        public INecromancerContext IsTakingTurn(bool isTakingTurn = true)
        {
            GetGame().Necromancer.IsTakingTurn = isTakingTurn;
            return this;
        }
    }
}
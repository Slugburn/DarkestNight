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
            var game = GetGame();
            game.Necromancer.Location = location.ToEnum<Location>();
            game.UpdatePlayerBoard();
            return this;
        }

        public INecromancerContext IsTakingTurn(bool isTakingTurn = true)
        {
            var necromancer = GetGame().Necromancer;
            if (!isTakingTurn)
                necromancer.IsTakingTurn = false;
            else
                necromancer.StartTurn();

            return this;
        }
    }
}
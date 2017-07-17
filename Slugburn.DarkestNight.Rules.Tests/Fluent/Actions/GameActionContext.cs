using System.IO;
using Slugburn.DarkestNight.Rules.IO;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public interface IGameActionContext : IWhen
    {
        IGameActionContext BlightDestroyed(string location, string blight);
        IGameActionContext NecromancerActs(IFakeContext rolls = null);
        IGameActionContext Saved();
        IGameActionContext Restored();
    }

    public class GameActionContext : WhenContext, IGameActionContext
    {
        private string _savedGame;

        public GameActionContext(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IGameActionContext BlightDestroyed(string location, string blightType)
        {
            var blightId = GetBlightId(location, blightType);
            GetGame().DestroyBlight(GetHero(null), blightId);
            return this;
        }

        public IGameActionContext NecromancerActs(IFakeContext rolls = null)
        {
            var game = GetGame();
            game.ActingHero = null;
            game.Necromancer.StartTurn();
            return this;
        }

        public IGameActionContext Saved()
        {
            var game = GetGame();
            var serializer = new GameSerializer();
            using (var writer = new StringWriter())
            {
                serializer.Write(game, writer);
                _savedGame = writer.ToString();
            }
            return this;
        }

        public IGameActionContext Restored()
        {
            var game = new Game();
            Set(game);
            var player = new FakePlayer(game);
            Set(player);
            game.AddPlayer(player);
            var serializer = new GameSerializer();
            using (var reader = new StringReader(_savedGame))
                serializer.Read(game, reader);
            game.UpdatePlayerBoard();
            return this;
        }
    }
}
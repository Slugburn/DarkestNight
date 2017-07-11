using System.IO;
using Newtonsoft.Json;

namespace Slugburn.DarkestNight.Rules.IO
{
    public class GameSerializer
    {
        public void Write(Game game, TextWriter writer)
        {
            var serializer = new JsonSerializer {NullValueHandling = NullValueHandling.Ignore};
            var gameData = GameData.Create(game);
            serializer.Serialize(writer, gameData);
        }

        public void Read(Game game, TextReader reader)
        {
            var serializer = new JsonSerializer();
            var gameData = (GameData) serializer.Deserialize(reader, typeof(GameData));
            game.RestoreFrom(gameData);
        }
    }
}

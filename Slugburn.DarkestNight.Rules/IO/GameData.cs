using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.IO
{
    public class GameData
    {
        public static GameData Create(Game game)
        {
            var data = new GameData
            {
                Darkness = game.Darkness,
                Heroes = game.Heroes.Select(h => h.GetData()).ToList(),
                NecromancerLocation = game.Necromancer.Location,
                Spaces = game.Board.Spaces.Select(SpaceData.Create).ToList(),
                EventDeck = game.Events.ToList(),
                MapDeck = game.Maps.Select(MapData.Create).ToList(),
                ArtifactDeck = game.ArtifactDeck.ToList()
            };
            return data;
        }


        public int Darkness { get; set; }
        public List<HeroData> Heroes { get; set; }
        public Location NecromancerLocation { get; set; }
        public List<SpaceData> Spaces { get; set; }
        public List<string> EventDeck { get; set; }
        public List<MapData> MapDeck { get; set; }
        public List<string> ArtifactDeck { get; set; }
    }
}
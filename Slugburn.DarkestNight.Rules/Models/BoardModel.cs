using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class BoardModel
    {
        public BoardModel(Game game)
        {
            Darkness = game.Darkness;
            Locations = game.Board.Spaces.Select(space => new LocationModel(space)).ToList();
        }

        public BoardModel()
        {
        }

        public int Darkness { get; set; }
        public List<LocationModel> Locations { get; set; }
    }
}
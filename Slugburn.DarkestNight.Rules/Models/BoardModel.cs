using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class BoardModel
    {
        public BoardModel(Game game)
        {
            Darkness = game.Darkness;
            Locations = game.Board.Spaces.Select(space => new LocationModel(game, space)).ToList();
        }

        public int Darkness { get; set; }
        public List<LocationModel> Locations { get; set; }
    }
}
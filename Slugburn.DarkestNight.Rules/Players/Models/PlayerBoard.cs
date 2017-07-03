using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerBoard
    {
        public PlayerBoard(Game game)
        {
            Darkness = game.Darkness;
            Locations = game.Board.Spaces.Select(space => new PlayerBoardLocation(game, space)).ToList();
        }

        public int Darkness { get; set; }
        public List<PlayerBoardLocation> Locations { get; set; }
    }
}
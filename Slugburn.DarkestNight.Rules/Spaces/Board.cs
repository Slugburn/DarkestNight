using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.Spaces
{
    public class Board
    {
        private Dictionary<Location, Space> _spaces;

        private Board()
        {
        }

        public static Board CreateFor(Game game)
        {
            var board = new Board();
            var spaces = new Space[] { new Monastery(), new Mountains(), new Castle(), new Swamp(), new Ruins(), new Forest(), new Village() };
            foreach (var space in spaces)
                space.Game = game;
            board._spaces = spaces.ToDictionary(space => space.Location);
            return board;
        }

        public Space this[Location location] => _spaces[location];

        public IEnumerable<Space> Spaces => _spaces.Values;
    }
}

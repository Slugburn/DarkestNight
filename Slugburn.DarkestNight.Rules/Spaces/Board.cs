using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.Spaces
{
    public class Board
    {
        private readonly Dictionary<Location, Space> _spaces;

        public Board()
        {
            _spaces = new Space[] {new Monastery(), new Mountains(), new Castle(), new Swamp(), new Ruins(), new Forest(), new Village()}
                .ToDictionary(space => space.Location);
        }

        public Space this[Location location] => _spaces[location];

        public IEnumerable<Space> Spaces => _spaces.Values;
    }
}

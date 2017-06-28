using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Spaces;

namespace Slugburn.DarkestNight.Rules
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

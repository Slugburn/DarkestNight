using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Spaces;

namespace Slugburn.DarkestNight.Rules
{
    public class Board
    {
        private readonly Dictionary<Location, ISpace> _spaces;

        public Board()
        {
            _spaces = new ISpace[] {new Monastery(), new Mountains(), new Castle(), new Swamp(), new Ruins(), new Forest(), new Village()}
                .ToDictionary(space => space.Location);
        }

        public ISpace this[Location location]
        {
            get { return _spaces[location]; }
        }

        public IEnumerable<ISpace> Spaces
        {
            get { return _spaces.Values; }
        }
    }
}

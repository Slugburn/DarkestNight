using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules
{
    class Map : IMap
    {
        private readonly Dictionary<Location, Blight> _blights;
        private readonly Dictionary<Location, Find> _finds;
        
        public Map(Blight[] blights, Find[] searches)
        {
            _blights = new Dictionary<Location, Blight>()
                       {
                           {Location.Mountains, blights[0]},
                           {Location.Castle, blights[1]},
                           {Location.Village, blights[2]},
                           {Location.Swamp, blights[3]},
                           {Location.Forest, blights[4]},
                           {Location.Ruins, blights[5]},
                           {Location.Monastery, blights[6]}
                       };
            _finds = new Dictionary<Location, Find>()
                        {
                            {Location.Mountains, searches[0]},
                            {Location.Castle, searches[1]},
                            {Location.Village, searches[2]},
                            {Location.Swamp, searches[3]},
                            {Location.Forest, searches[4]},
                            {Location.Ruins, searches[5]},
                        };
        }

        public Blight GetBlight(Location location)
        {
            return _blights[location];
        }

        public Find GetSearchResult(Location location)
        {
            return _finds[location];
        }
    }
}
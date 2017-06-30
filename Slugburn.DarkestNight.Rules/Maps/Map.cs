using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Maps
{
    public class Map : IMap
    {
        private readonly Dictionary<Location, BlightType> _blights;
        private readonly Dictionary<Location, Find> _finds;
        
        public Map(BlightType[] blightType, Find[] searches)
        {
            _blights = new Dictionary<Location, BlightType>()
                       {
                           {Location.Mountains, blightType[0]},
                           {Location.Castle, blightType[1]},
                           {Location.Village, blightType[2]},
                           {Location.Swamp, blightType[3]},
                           {Location.Forest, blightType[4]},
                           {Location.Ruins, blightType[5]},
                           {Location.Monastery, blightType[6]}
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

        public BlightType GetBlight(Location location)
        {
            return _blights[location];
        }

        public Find GetSearchResult(Location location)
        {
            return _finds[location];
        }
    }
}
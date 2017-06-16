using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Maps
{
    class Map : IMap
    {
        private readonly Dictionary<Location, Blight> _blights;
        private readonly Dictionary<Location, Find> _finds;
        
        public Map(Blight[] blight, Find[] searches)
        {
            _blights = new Dictionary<Location, Blight>()
                       {
                           {Location.Mountains, blight[0]},
                           {Location.Castle, blight[1]},
                           {Location.Village, blight[2]},
                           {Location.Swamp, blight[3]},
                           {Location.Forest, blight[4]},
                           {Location.Ruins, blight[5]},
                           {Location.Monastery, blight[6]}
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
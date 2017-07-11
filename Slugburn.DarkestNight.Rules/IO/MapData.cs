using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Maps;

namespace Slugburn.DarkestNight.Rules.IO
{
    public class MapData
    {
        public static MapData Create(IMap map)
        {
            var blights = new Dictionary<Location, BlightType>();
            var finds = new Dictionary<Location, Find>();
            var mapLocations = new[] {Location.Monastery,  Location.Castle, Location.Forest, Location.Mountains, Location.Ruins, Location.Swamp, Location.Village };
            foreach (var location in mapLocations)
            {
                blights[location] = map.GetBlight(location);
                if (location != Location.Monastery)
                    finds[location] = map.GetSearchResult(location);
            }
            return new MapData
            {
                Blights = blights,
                Finds = finds
            };
        }

        public Dictionary<Location,BlightType> Blights { get; set; }
        public Dictionary<Location, Find> Finds { get; set; }
    }
}
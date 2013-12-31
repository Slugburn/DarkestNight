using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Spaces
{
    class Village : Space
    {
        public Village()
        {
            Location = Location.Village;
            Name = "Village";
            SearchTarget = 3;
            HasRelic = false;
            AdjacentLocations = new[] { Location.Monastery, Location.Mountains, Location.Castle, Location.Swamp, Location.Ruins, Location.Forest };
            MoveChart = new Dictionary<int, Location>
            {
                {1, Location.Mountains},
                {2, Location.Ruins},
                {3, Location.Forest},
                {4, Location.Castle},
                {5, Location.Swamp},
                {6, Location.Village},
            };
        }
    }
}
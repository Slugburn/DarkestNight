using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Spaces
{
    class Forest : Space
    {
        public Forest()
        {
            Location = Location.Forest;
            Name = "Forest";
            SearchTarget = 4;
            HasRelic = true;
            AdjacentLocations = new[] { Location.Monastery, Location.Village, Location.Ruins };
            MoveChart = new Dictionary<int, Location>
            {
                {1, Location.Village},
                {2, Location.Ruins},
                {3, Location.Forest},
                {4, Location.Village},
                {5, Location.Ruins},
                {6, Location.Forest},
            };
        }
    }
}
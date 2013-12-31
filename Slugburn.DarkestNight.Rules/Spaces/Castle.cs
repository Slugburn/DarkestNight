using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Spaces
{
    class Castle : Space
    {
        public Castle()
        {
            Location = Location.Castle;
            Name = "Castle";
            SearchTarget = 2;
            HasRelic = false;
            AdjacentLocations = new[] { Location.Mountains, Location.Village, Location.Swamp };
            MoveChart = new Dictionary<int, Location>
            {
                {1, Location.Swamp},
                {2, Location.Village},
                {3, Location.Mountains},
                {4, Location.Castle},
                {5, Location.Village},
                {6, Location.Castle},
            };
        }
    }
}
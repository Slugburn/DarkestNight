using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Spaces
{
    class Ruins : Space
    {
        public Ruins()
        {
            Location = Location.Ruins;
            Name = "Ruins";
            SearchTarget = 4;
            HasRelic = true;
            AdjacentLocations = new[] { Location.Forest, Location.Village, Location.Swamp };
            MoveChart = new Dictionary<int, Location>
            {
                {1, Location.Forest},
                {2, Location.Swamp},
                {3, Location.Village},
                {4, Location.Ruins},
                {5, Location.Village},
                {6, Location.Ruins},
            };
        }
    }
}
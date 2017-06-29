using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Actions;

namespace Slugburn.DarkestNight.Rules.Spaces
{
    class Swamp : Space
    {
        public Swamp()
        {
            Location = Location.Swamp;
            Name = "Swamp";
            SearchTarget = 4;
            HasRelic = true;
            AdjacentLocations = new[] { Location.Castle, Location.Village, Location.Ruins };
            MoveChart = new Dictionary<int, Location>
            {
                {1, Location.Ruins},
                {2, Location.Village},
                {3, Location.Castle},
                {4, Location.Swamp},
                {5, Location.Village},
                {6, Location.Swamp},
            };
            AddAction(new RetrieveRelic());
        }
    }
}
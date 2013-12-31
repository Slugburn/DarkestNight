using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Spaces
{
    class Mountains : Space
    {
        public Mountains()
        {
            Location = Location.Mountains;
            Name = "Mountains";
            SearchTarget = 4;
            HasRelic = true;
            AdjacentLocations = new[] {Location.Monastery, Location.Village, Location.Castle};
            MoveChart = new Dictionary<int, Location>
            {
                {1, Location.Castle},
                {2, Location.Village},
                {3, Location.Mountains},
                {4, Location.Village},
                {5, Location.Castle},
                {6, Location.Mountains},
            };
        }
    }
}

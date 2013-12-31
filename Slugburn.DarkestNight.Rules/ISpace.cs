using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules
{
    public interface ISpace
    {
        Location Location { get; }
        string Name { get; }
        int SearchTarget { get; set; }
        IEnumerable<Location> AdjacentLocations { get; }
        IEnumerable<IBlight> Blights { get; }
        IDictionary<int, Location> MoveChart { get; }

        bool HasRelic { get; set; }

        void AddBlight(IBlight blight);
        void RemoveBlight(IBlight blight);
    }
}

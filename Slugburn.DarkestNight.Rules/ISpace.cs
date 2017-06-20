using System.Collections;
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
        ICollection<Blight> Blights { get; }
        IDictionary<int, Location> MoveChart { get; }

        bool HasRelic { get; set; }

        void AddBlight(Blight blight);
        void Add<T>(T item);
        void Remove<T>(T item);
        Blight GetBlight(Blight type);
    }
}

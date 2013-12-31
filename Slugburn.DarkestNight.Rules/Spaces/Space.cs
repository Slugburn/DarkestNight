using System;
using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Spaces
{
    public abstract class Space : ISpace
    {
        public Location Location { get; protected set; }
        public string Name { get; protected set; }
        public int SearchTarget { get; set; }
        public IEnumerable<Location> AdjacentLocations { get; protected set; }
        public IEnumerable<IBlight> Blights { get; private set; }
        public IDictionary<int, Location> MoveChart { get; protected set; }
        public bool HasRelic { get; set; }
        public void AddBlight(IBlight blight)
        {
            throw new NotImplementedException();
        }

        public void RemoveBlight(IBlight blight)
        {
            throw new NotImplementedException();
        }
    }
}

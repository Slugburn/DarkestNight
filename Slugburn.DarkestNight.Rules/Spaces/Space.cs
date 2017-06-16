using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Spaces
{
    public abstract class Space : ISpace
    {
        private readonly List<IBlight> _blights;

        protected Space()
        {
            _blights = new List<IBlight>();
        }

        public Location Location { get; protected set; }
        public string Name { get; protected set; }
        public int SearchTarget { get; set; }
        public IEnumerable<Location> AdjacentLocations { get; protected set; }

        public IEnumerable<IBlight> Blights
        {
            get { return _blights; }
        }

        public IDictionary<int, Location> MoveChart { get; protected set; }
        public bool HasRelic { get; set; }
        
        public void AddBlight(IBlight blight)
        {
            _blights.Add(blight);
        }

        public void RemoveBlight(IBlight blight)
        {
            _blights.Remove(blight);
        }

        public void Add<T>(T item)
        {
            throw new NotImplementedException();
        }

        public void Remove<T>(T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetBlights<T>() where T: IBlight
        {
            return _blights.Where(x=>x is T).Cast<T>();
        }
    }
}

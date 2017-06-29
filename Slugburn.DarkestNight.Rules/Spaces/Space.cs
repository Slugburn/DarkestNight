using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Spaces
{
    public abstract class Space
    {
        private readonly List<Blight> _blights = new List<Blight>();
        private readonly Dictionary<string, IAction> _actions = new Dictionary<string, IAction>();


        public Location Location { get; protected set; }
        public string Name { get; protected set; }
        public int SearchTarget { get; set; }
        public IEnumerable<Location> AdjacentLocations { get; protected set; }

        public ICollection<Blight> Blights => _blights;

        public IDictionary<int, Location> MoveChart { get; protected set; }
        public bool HasRelic { get; set; }
        
        public void AddBlight(Blight blight)
        {
            _blights.Add(blight);
        }

        public void RemoveBlight(Blight blight)
        {
            _blights.Remove(blight);
        }

        public IEnumerable<T> GetBlights<T>() where T: IBlightDetail
        {
            return _blights.Where(x=>x is T).Cast<T>();
        }

        public Blight GetBlight(Blight type)
        {
            return _blights.FirstOrDefault(x => x == type);
        }

        public IEnumerable<IAction> GetActions()
        {
            return _actions.Values.ToList();
        }

        public IAction GetAction(string actionName)
        {
            return _actions.ContainsKey(actionName) ? _actions[actionName] : null;
        }

        public void AddAction(IAction action)
        {
            _actions.Add(action.Name, action);
        }

        public void RemoveAction(string actionName)
        {
            if (!_actions.Remove(actionName))
                throw new ArgumentOutOfRangeException(actionName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Spaces
{
    public abstract class Space
    {
        private readonly List<IBlight> _blights = new List<IBlight>();
        private readonly Dictionary<string, IAction> _actions = new Dictionary<string, IAction>();
        private int _baseSearchTarget;


        public Location Location { get; protected set; }
        public string Name { get; protected set; }

        public int GetSearchTarget(Hero hero)
        {
            var darkFogs = GetActiveBlights<DarkFog>(hero).Count();
            return _baseSearchTarget + darkFogs*2;
        }

        public int SearchTarget
        {
            set { _baseSearchTarget = value; }
        }

        public IEnumerable<T> GetActiveBlights<T>(Hero hero = null) where T:IBlight
        {
            return GetBlights<T>().Where(b=>!Game.IsBlightSupressed(b, hero));
        }

        public IEnumerable<Location> AdjacentLocations { get; protected set; }

        public ICollection<IBlight> Blights => _blights;

        public IDictionary<int, Location> MoveChart { get; protected set; }
        public bool HasRelic { get; set; }
        public Game Game { get; set; }

        public void AddBlight(IBlight blight)
        {
            _blights.Add(blight);
            ((Blight)blight).SetSpace(this);
        }

        public void RemoveBlight(IBlight blight)
        {
            _blights.Remove(blight);
            ((Blight)blight).SetSpace(null);
        }

        public IEnumerable<T> GetBlights<T>() where T: IBlight
        {
            return _blights.Where(x=>x is T).Cast<T>();
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

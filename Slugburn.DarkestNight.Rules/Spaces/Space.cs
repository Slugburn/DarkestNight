﻿using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Spaces
{
    public abstract class Space
    {
        private readonly List<IBlight> _blights = new List<IBlight>();
        private readonly Dictionary<string, IAction> _actions = new Dictionary<string, IAction>();
        private readonly List<IModifier> _modifiers = new List<IModifier>();
        private int? _baseSearchTarget;
        private readonly List<Effect> _effects = new List<Effect>();


        public Location Location { get; protected set; }
        public string Name { get; protected set; }

        public int? GetSearchTarget(Hero hero)
        {
            if (_baseSearchTarget == null) return null;
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
            if (_blights.Count < 4)
            {
                _blights.Add(blight);
                ((Blight) blight).SetSpace(this);
            }
            else
            {
                if (Location == Location.Monastery)
                    Game.Lose();
                else
                    Game.Board[Location.Monastery].AddBlight(blight);
            }
        }

        public void RemoveBlight(IBlight blight)
        {
            _blights.Remove(blight);
            ((Blight)blight).SetSpace(null);
        }

        public IEnumerable<T> GetBlights<T>() where T: IBlight
        {
            return _blights.OfType<T>();
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
            AddEffect(action.Name, action.Text);
        }

        public void RemoveAction(string actionName)
        {
            if (!_actions.Remove(actionName))
                throw new ArgumentOutOfRangeException(actionName);
            _effects.RemoveAll(x => x.Name == actionName);
        }

        public void AddModifier(IModifier modifier, string effectText)
        {
            _modifiers.Add(modifier);
            AddEffect(modifier.Name, effectText);
        }

        public IEnumerable<IModifier> GetModifiers() => _modifiers.ToList();

        public void AddEffect(string name, string text)
        {
            _effects.Add(new Effect {Name = name, Text = text});
            Game?.UpdatePlayerBoard();
        }

        public IEnumerable<Effect> GetEffects() => _effects;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class FakePlayer : IPlayer
    {
        private readonly Dictionary<string, bool> _usePowerResponse;
        private readonly List<Tuple<Blight, int>> _blightRollAssignments;

        private string _tacticChoice;
        private Blight[] _blightChoice;
        private Location? _locationChoice;
        private Queue<bool> _rollAnotherDie;

        public FakePlayer()
        {
            _usePowerResponse = new Dictionary<string, bool>();
            _blightRollAssignments = new List<Tuple<Blight, int>>();
            _rollAnotherDie = new Queue<bool>();
        }

        public bool AskUsePower(string name, string description)
        {
            if (_usePowerResponse.ContainsKey(name))
                return _usePowerResponse[name];
            return false;
        }

        public void SetUsePowerResponse(string name, bool value)
        {
            _usePowerResponse[name] = value;
        }


        public void SetTacticChoice(string powerName)
        {
            _tacticChoice = powerName;
        }


        public void SetBlightChoice(Blight[] blights)
        {
            _blightChoice = blights;
        }

        public void SetBlightRollAssignment(Blight blight, int roll)
        {
            _blightRollAssignments.Add(Tuple.Create(blight, roll));
        }

        public List<Blight> ChooseBlights(ICollection<Blight> choices, int min, int max)
        {
            if (CancelBlightSelectionSpecified())
                return new List<Blight>();
            var choice = ChooseBlights(choices);
            if (choice.Count < min || choice.Count > max)
                throw new Exception("Invalid choices have been specified for IPlayer.ChooseBlights().");
            return choice;
        }

        private List<Blight> ChooseBlights(ICollection<Blight> choices)
        {
            if (_blightChoice == null || _blightChoice.Except(choices).ToList().Any())
                throw new Exception("Invalid choices have been specified for IPlayer.ChooseBlights().");
            var choice = choices.Intersect(_blightChoice).ToList();
            return choice;
        }

        private bool CancelBlightSelectionSpecified()
        {
            return _blightChoice != null && _blightChoice.Length == 1 && _blightChoice[0] == Blight.None;
        }


        public void SetLocationChoice(Location location)
        {
            _locationChoice = location;
        }

        public Location ChooseLocation(IEnumerable<Location> choices)
        {
            if (_locationChoice == Location.None) return Location.None;
            var choice = choices.SingleOrDefault(x => x == _locationChoice);
            if (choice == Location.None)
                throw new Exception("No valid choice was specified for IPlayer.ChooseLocation().");
            return choice;
        }

        public void SetRollAnotherDieChoice(bool[] choices)
        {
            foreach (var choice in choices)
                _rollAnotherDie.Enqueue(choice);
        }
    }
}

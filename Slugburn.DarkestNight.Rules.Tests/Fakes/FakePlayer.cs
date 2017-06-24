using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;

namespace Slugburn.DarkestNight.Rules.Tests.Fakes
{
    public class FakePlayer : IPlayer
    {
        private readonly List<Tuple<Blight, int>> _blightRollAssignments;
        private readonly Game _game;
        private readonly Dictionary<string, bool> _usePowerResponse;
        private Blight[] _blightChoice;
        private Location? _locationChoice;
        private readonly Queue<bool> _rollAnotherDie;

        private string _tacticChoice;

        public FakePlayer(Game game)
        {
            _game = game;
            _usePowerResponse = new Dictionary<string, bool>();
            _blightRollAssignments = new List<Tuple<Blight, int>>();
            _rollAnotherDie = new Queue<bool>();
        }

        public PlayerConflict Conflict { get; set; }

        public PlayerEvent Event { get; set; }
        public string ActiveHero { get; set; }
        public ICollection<PlayerPower> Powers { get; set; }
        public ICollection<PlayerBlight> Blights { get; set; }
        public ICollection<string> ValidLocations { get; set; }

        public bool AskUsePower(string name, string description)
        {
            if (_usePowerResponse.ContainsKey(name))
                return _usePowerResponse[name];
            return false;
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

        public Location ChooseLocation(IEnumerable<Location> choices)
        {
            if (_locationChoice == Location.None) return Location.None;
            var choice = choices.SingleOrDefault(x => x == _locationChoice);
            if (choice == Location.None)
                throw new Exception("No valid choice was specified for IPlayer.ChooseLocation().");
            return choice;
        }

        public PlayerState State { get; set; }

        public void DisplayEvent(PlayerEvent playerEvent)
        {
            Event = playerEvent;
        }

        public void DisplayConflict(PlayerConflict conflict)
        {
            Conflict = conflict;
        }

        public void DisplayPowers(ICollection<PlayerPower> powers)
        {
            Powers = powers;
        }

        public void DisplayBlights(ICollection<PlayerBlight> blights)
        {
            Blights = blights;
        }

        public void DisplayValidLocations(ICollection<string> locations)
        {
            ValidLocations = locations;
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

        public void SetRollAnotherDieChoice(bool[] choices)
        {
            foreach (var choice in choices)
                _rollAnotherDie.Enqueue(choice);
        }

        public void SelectEventOption(string option)
        {
            var matching = Event.Options.SingleOrDefault(x => x.Text == option);
            matching.ShouldNotBeNull($"No matching event with Text '{option}'.");
            var code = matching?.Code ?? "<unknown>";
            _game.ActingHero.SelectEventOption(code);
        }

        public void AcceptRoll()
        {
            _game.ActingHero.AcceptRoll();
        }

        public void TakeAction(string heroName, string actionName)
        {
            var hero = _game.GetHero(heroName);
            var action = hero.GetAction(actionName);
            action.Act(hero);
        }

        public void SelectLocation(Location location)
        {
            var hero = _game.GetHero(ActiveHero);
            hero.SelectLocation(location);
        }

        public void ResolveConflict(string tacticName, ICollection<int> targetIds)
        {
            var hero = _game.GetHero(ActiveHero);
            hero.SelectTactic(tacticName, targetIds);
        }

        public void SelectPower(string powerName)
        {
            _game.SelectPower(powerName);
        }

        public void SelectBlight(Location location, Blight blight)
        {
            _game.SelectBlight(location, blight);
        }
    }
}
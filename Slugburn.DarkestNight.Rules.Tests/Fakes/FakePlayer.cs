﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Tests.Fakes
{
    public class FakePlayer : IPlayer
    {
        public Game Game { get; set; }

        private object _callback;
        private TaskCompletionSource<string> _submitAnswer;
        private TaskCompletionSource<Location> _locationSource;
        private TaskCompletionSource<IEnumerable<int>> _blightsSource;
        private TaskCompletionSource<Hero> _heroSource;
        private TaskCompletionSource<string> _powerSource;
        private TaskCompletionSource<MoveResponse> _moveResponseSource;

        public FakePlayer(Game game)
        {
            Game = game;
        }

        public BlightSelectionModel BlightSelection { get; set; }
        public ConflictModel Conflict { get; set; }
        public EventModel Event { get; set; }
        public ICollection<PowerModel> Powers { get; set; }
        public ICollection<string> ValidLocations { get; set; }
        public NecromancerModel Necromancer { get; set; }

        public HeroSelectionModel HeroSelection { get; set; }

        public PlayerState State { get; set; }

        public void DisplayEvent(EventModel playerEvent)
        {
            Event = playerEvent;
        }

        public void DisplayConflict(ConflictModel conflict)
        {
            Conflict = conflict;
        }

        public Task<string> SelectPower(ICollection<PowerModel> models)
        {
            Powers = models;
            _powerSource = new TaskCompletionSource<string>();
            return _powerSource.Task;
        }

        public Task<IEnumerable<int>> SelectBlights(BlightSelectionModel model)
        {
            BlightSelection = model;
            _blightsSource = new TaskCompletionSource<IEnumerable<int>>();
            return _blightsSource.Task;
        }

        public Task<Location> SelectLocation(ICollection<string> locations)
        {
            ValidLocations = locations;
            _locationSource = new TaskCompletionSource<Location>();
            return _locationSource.Task;
        }

        public void DisplayNecromancer(NecromancerModel model)
        {
            Necromancer = model;
        }

        public Task<Hero> SelectHero(HeroSelectionModel model)
        {
            HeroSelection = model;
            _heroSource = new TaskCompletionSource<Hero>();
            return _heroSource.Task;
        }

        Task<string> IPlayer.AskQuestion(QuestionModel model)
        {
            AskQuestion = model;
            _submitAnswer = new TaskCompletionSource<string>();
            return _submitAnswer.Task;
        }

        public void DisplaySearch(SearchModel model, Callback<Find> callback)
        {
            Search = model;
            _callback = callback;
        }

        public void DisplayPrayer(PrayerModel model)
        {
            Prayer = model;
        }

        public void AddHero(HeroModel view)
        {
            Heroes.Add(view);
        }

        public void UpdateBoard(BoardModel model)
        {
            Board = model;
        }

        public void UpdateHeroCommands(HeroActionModel model)
        {
            var hero = Heroes.Single(x => x.Name == model.HeroName);
            hero.Commands = model.Commands.ToList();
            hero.Inventory = model.Items.ToList();
        }

        public void UpdateHeroStatus(string heroName, HeroStatusModel status)
        {
            Heroes.Single(x => x.Name == heroName).Status = status;
        }

        public void OnNewDay()
        {
        }

        public void StopMoving()
        {
            _moveResponseSource.SetResult(new MoveResponse {Stop = true});
        }

        public Task<MoveResponse> SelectDestination(List<string> validLocations)
        {
            ValidLocations = validLocations;
            _moveResponseSource = new TaskCompletionSource<MoveResponse>();
            return _moveResponseSource.Task;
        }

        public QuestionModel AskQuestion { get; set; }
        public SearchModel Search { get; set; }
        public PrayerModel Prayer { get; set; }
        public List<HeroModel> Heroes { get; set; } = new List<HeroModel>();
        public BoardModel Board { get; set; }
        
        public void SelectEventOption(string option)
        {
            var matching = Event.Options.SingleOrDefault(x => x.Text == option);
            matching.ShouldNotBeNull($"No matching event with Text '{option}'.");
            var code = matching?.Code ?? "<unknown>";
            Game.ActingHero.SelectEventOption(code);
        }

        public void AcceptRoll()
        {
            AcceptedRoll = Game.ActingHero.CurrentRoll.AdjustedRoll;
            Game.ActingHero.AcceptRoll();
        }

        public List<int> AcceptedRoll { get; set; }

        public void ExecuteCommand(string heroName, string commandName)
        {
            var hero = Game.GetHero(heroName);
            hero.ExecuteCommand(commandName);
        }

        public void SelectLocation(Location location)
        {
            _locationSource.SetResult(location);
        }

        public void ResolveConflict( string tacticName, ICollection<int> targetIds)
        {
            var hero = Game.ActingHero;
            hero.SelectTactic(tacticName, targetIds);
        }

        public void SelectPower(string powerName)
        {
            _powerSource.SetResult(powerName);
        }

        public void SelectBlight(int blightId)
        {
            _blightsSource.SetResult(new [] {blightId});
        }

        public void FinishNecromancerTurn()
        {
            Game.Necromancer.CompleteTurn();
        }

        public void AssignDiceToTargets(List<TargetDieAssignment> diceAssignment)
        {
            var hero = Game.ActingHero;
            hero.AssignDiceToTargets(diceAssignment);
        }

        public void SelectBlights(IEnumerable<int> blightIds)
        {
            _blightsSource.SetResult(blightIds);
        }

        public void SelectHero(string heroName)
        {
            var selectedHero = Game.GetHero(heroName);
            _heroSource.SetResult(selectedHero);
        }

        public void AnswerQuestion(string answer)
        {
            if (_submitAnswer != null)
            {
                _submitAnswer.SetResult(answer);
            }
            else if (_callback != null)
            {
                var callback = (Callback<string>)_callback;
                callback.Handle(answer);
            }
        }

        public void SelectSearchResult(Find code)
        {
            var callback = (Callback<Find>) _callback;
            callback.Handle(code);
        }

        internal void SelectDestination(Location location)
        {
            _moveResponseSource.SetResult(new MoveResponse {Destination = location});
        }

        public void TradeItem(int itemId, string fromHeroName, string toHeroName)
        {
            var fromHero = Game.GetHero(fromHeroName);
            var toHero = Game.GetHero(toHeroName);
            fromHero.TradeItemTo(toHero, itemId);
        }
    }
}
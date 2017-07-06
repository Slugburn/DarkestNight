using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public class PlayerActionContext : WhenContext, IPlayerActionContext
    {
        private List<TargetDieAssignment> _diceAssignment;
        private string[] _selectedTargets;
        private string _selectedTactic;

        public PlayerActionContext(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IPlayerActionContext UsesTactic(string tacticName)
        {
            _selectedTactic = tacticName;
            return this;
        }

        public IPlayerActionContext TakesAction(string actionName, IFakeContext fake = null)
        {
            return TakesAction(null, actionName, fake);
        }

        public IPlayerActionContext TakesAction(string heroName, string actionName, IFakeContext fake = null)
        {
            if (heroName == null)
            {
                var hero = GetHero(null);
                heroName = hero.Name;
            }

            GetPlayer().ExecuteCommand(heroName, actionName);
            return this;
        }
        
        public IPlayerActionContext SelectsEventOption(string option, IFakeContext set = null)
        {
            GetPlayer().SelectEventOption(option);
            return this;
        }

        public IPlayerActionContext AcceptsRoll()
        {
            var hero = this.GetHero(null);
            var roll = hero.CurrentRoll;
            Set(roll);
            GetPlayer().AcceptRoll();
            return this;
        }

        public IPlayerActionContext SelectsLocation(string location)
        {
            GetPlayer().SelectLocation(location.ToEnum<Location>());
            return this;
        }

        public IPlayerActionContext ResolvesConflict(IFakeContext fake = null)
        {
            // verify targets
            var availableTargets = GetPlayer().Conflict.Targets.ToList();
            var availableTargetsNames = availableTargets.Select(x => x.Name).ToArray();

            if (!_selectedTargets.Any() || _selectedTargets.First() == null)
            {
                if (availableTargets.Count == 1)
                    _selectedTargets = availableTargetsNames;
                else
                    Assert.Fail($"Please select a target. Available targets are '{availableTargetsNames.ToCsv()}'.");
            }

            var verified = _selectedTargets.Intersect(availableTargetsNames).ToList();
            if (verified.Count != _selectedTargets.Length)
                Assert.Fail($"Selected targets '{_selectedTargets.ToCsv()}' are not valid. Available targets are '{availableTargetsNames.ToCsv()}'.");
            var targetIds = _selectedTargets.Select(tn =>
            {
                var target = availableTargets.First(t => t.Name == tn);
                availableTargets.Remove(target);
                return target.Id;
            }).ToList();

            _selectedTactic = _selectedTactic ?? "Fight";
            var availableTactics = GetPlayer().Conflict.Tactics.Select(x => x.Name).ToList();
            if (!availableTactics.Contains(_selectedTactic))
                Assert.Fail($"Selected tactic '{_selectedTactic}' is not valid. Available tactics are '{availableTactics.ToCsv()}'.");

            GetPlayer().ResolveConflict(_selectedTactic, targetIds);
            return this;
        }

        public IPlayerActionContext SelectsPower(string powerName)
        {
            GetPlayer().SelectPower(powerName);
            return this;
        }

        public IPlayerActionContext Targets(params string[] targetNames)
        {
            _selectedTargets = targetNames;
            return this;
        }

        public IPlayerActionContext SelectsBlight(string location, string blightType)
        {
            var blightId = GetBlightId(location, blightType);
            GetPlayer().SelectBlight(blightId);
            return this;
        }

        public IPlayerActionContext AcceptsNecromancerTurn()
        {
            GetPlayer().FinishNecromancerTurn();
            return this;
        }

        public IPlayerActionContext AssignsDie(int dieValue, string targetName)
        {
            var conflict = GetPlayer().Conflict;
            var target = conflict.Targets.SingleOrDefault(x => x.Name == targetName);
            if (target==null)
                Assert.Fail($"{targetName} is not a valid target.");
            if (_diceAssignment == null)
                _diceAssignment = new List<TargetDieAssignment>();
            _diceAssignment.Add(new TargetDieAssignment {TargetId = target.Id, DieValue = dieValue});
            if (_diceAssignment.Count == conflict.TargetCount)
                GetPlayer().AssignDiceToTargets(_diceAssignment);
            return this;
        }

        public IPlayerActionContext AcceptsConflictResults(int count = 1)
        {
            for (var i = 0; i < count; i++)
                GetPlayer().AcceptConflictResult();
            return this;
        }

        public IPlayerActionContext SelectsBlights(params string[] blights)
        {
            var hero = GetGame().ActingHero;
            var blightIds = blights.Select(b => GetBlightId(hero.Location.ToString(), b));
            GetPlayer().SelectBlights(blightIds);
            return this;
        }

        public IPlayerActionContext SelectsHero(string heroName)
        {
            GetPlayer().SelectHero(heroName);
            return this;
        }

        public IPlayerActionContext AnswersQuestion(string title, string answer)
        {
            var player = GetPlayer();
            player.AskQuestion.ShouldNotBeNull("No question has been asked.");
            player.AskQuestion.Title.ShouldBe(title);
            player.AnswerQuestion(answer);
            return this;
        }

        public IPlayerActionContext SelectsSearchResult(string resultName = null)
        {
            var player = GetPlayer();
            var possibleResults = player.Search.SearchResults;
            resultName = resultName !=null ? (possibleResults.Single(x=>x.Name == resultName).Code) : possibleResults.Single().Code;
            player.SelectSearchResult(resultName);
            return this;
        }

        public IPlayerActionContext TradesItem(string itemName, string fromHeroName, string toHeroName)
        {
            var game = GetGame();
            var item = game.GetHero(fromHeroName).GetInventory().FirstOrDefault(i => i.Name == itemName);
            if (item == null)
                throw new ArgumentOutOfRangeException(nameof(itemName), itemName);
            var itemId = item.Id;
            GetPlayer().TradeItem(itemId, fromHeroName, toHeroName);
            return this;
        }
    }
}
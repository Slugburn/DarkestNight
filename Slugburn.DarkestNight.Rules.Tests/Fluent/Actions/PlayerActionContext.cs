using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;

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

        public IPlayerActionContext TakesAction(string actionName)
        {
            GetPlayer().TakeAction(GetPlayer().ActiveHero, actionName);
            return this;
        }


        public IPlayerActionContext ChoosesBlight(params string[] blights)
        {
            GetPlayer().SetBlightChoice(blights.Select(x => x.ToEnum<Blight>()).ToArray());
            return this;
        }

        public IPlayerActionContext ChooseLocation(Location location)
        {
            GetPlayer().SetLocationChoice(location);
            return this;
        }

        public IPlayerActionContext SelectsEventOption(string option, IFakeContext set = null)
        {
            GetPlayer().SelectEventOption(option);
            return this;
        }

        public IPlayerActionContext AcceptsRoll()
        {
            GetPlayer().AcceptRoll();
            return this;
        }

        public IPlayerActionContext SelectsLocation(string location)
        {
            GetPlayer().SelectLocation(location.ToEnum<Location>());
            return this;
        }

        public IPlayerActionContext ResolvesConflict(Action<ResolveConflictContext> action)
        {
            var context = new ResolveConflictContext(GetPlayer().Conflict);
            action(context);
            GetPlayer().ResolveConflict(context.GetTactic(), context.GetTargetIds());
            return this;
        }

        public IPlayerActionContext ResolvesConflict(IFakeContext fake = null)
        {
            // verify targets
            var availableTargets = GetPlayer().Conflict.Targets.ToList();
            var availableTargetsNames = availableTargets.Select(x => x.Name).ToList();
            var verified = _selectedTargets.Intersect(availableTargetsNames).ToList();
            if (verified.Count != _selectedTargets.Length)
                Assert.Fail($"Selected targets '{_selectedTargets.ToCsv()}' are not valid. Available targets are '{availableTargetsNames.ToCsv()}'.");
            var targetIds = _selectedTargets.Select(tn =>
            {
                var target = availableTargets.First(t => t.Name == tn);
                availableTargets.Remove(target);
                return target.Id;
            }).ToList();

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

        public IPlayerActionContext SelectsBlight(string location, string blight)
        {
            GetPlayer().SelectBlight(location.ToEnum<Location>(), blight.ToEnum<Blight>());
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

        public IPlayerActionContext AcceptsConflictResults()
        {
            while (GetGame().ActingHero.ConflictState.SelectedTargets.Any())
                GetPlayer().AcceptConflictResult();
            return this;
        }
    }
}
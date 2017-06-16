using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class FakePlayer : IPlayer
    {
        private readonly Dictionary<string, bool> _usePowerResponse;
        private readonly Queue<int> _upcomingRolls;
        private string _tacticChoice;
        private int _dieCountChoice;
        private BlightType[] _blightChoice;
        private List<Tuple<BlightType, int>> _blightRollAssignments;

        public FakePlayer()
        {
            _usePowerResponse = new Dictionary<string, bool>();
            _upcomingRolls = new Queue<int>();
            _blightRollAssignments = new List<Tuple<BlightType, int>>();
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


        public int RollOne()
        {
            return _upcomingRolls.Dequeue();
        }

        public void AddUpcomingRoll(int value)
        {
            _upcomingRolls.Enqueue(value);
        }

        public void AddUpcomingRolls(int[] dieRolls)
        {
            foreach (var roll in dieRolls)
                _upcomingRolls.Enqueue(roll);
        }

        public void SetTacticChoice(string powerName)
        {
            _tacticChoice = powerName;
        }

        public void SetNumberOfDiceChoice(int count)
        {
            _dieCountChoice = count;
        }

        public Tactic ChooseTactic(IEnumerable<Tactic> choices)
        {
            return choices.Single(x=>x.Name==_tacticChoice);
        }

        public int ChooseDieCount(params int[] choices)
        {
            return choices.Single(x => x == _dieCountChoice);
        }

        public IEnumerable<int> RollDice(int count)
        {
            if (count > _upcomingRolls.Count)
                throw new Exception($"Rolling {count} dice but only {_upcomingRolls.Count} specified rolls are remaining.");
            for (var i = 0; i < count; i++)
                yield return _upcomingRolls.Dequeue();
        }

        public void SetBlightChoice(BlightType[] blights)
        {
            _blightChoice = blights;
        }

        public void SetBlightRollAssignment(BlightType blight, int roll)
        {
            _blightRollAssignments.Add(Tuple.Create(blight, roll));
        }

        public List<BlightType> ChooseBlights(List<BlightType> choices, int count)
        {
            return choices.Intersect(_blightChoice).ToList();
        }

        public int AssignRollToBlight(BlightType blight, List<int> rolls)
        {
            var assignment = _blightRollAssignments.FirstOrDefault(x => x.Item1 == blight && rolls.Contains(x.Item2));
            if (assignment == null)
                throw new Exception($"No valid roll has been specified for {blight}");
            _blightRollAssignments.Remove(assignment);
            return assignment.Item2;
        }
    }
}

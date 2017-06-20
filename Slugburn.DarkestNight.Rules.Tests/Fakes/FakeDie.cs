using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Tests.Fakes
{
    public class FakeDie : IDie
    {
        private readonly Queue<int> _upcomingRolls = new Queue<int>();

        public int Roll()
        {
            var roll = _upcomingRolls.Any() ? _upcomingRolls.Dequeue() : 6;
            return roll;
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

    }
}

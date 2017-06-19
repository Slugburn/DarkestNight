using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class FakeDie : IDie
    {
        private readonly Queue<int> _upcomingRolls = new Queue<int>();

        public int Roll()
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

    }
}

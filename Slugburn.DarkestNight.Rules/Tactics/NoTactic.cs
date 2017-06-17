using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tactics
{
    public class NoTactic : ITactic
    {
        public string Name => "None";

        public bool IsAvailable(Hero hero) => true;

        public int GetDiceCount() => 1;

        public void AfterRoll(Hero hero, ICollection<int> roll)
        {
            // do nothing
        }
    }
}

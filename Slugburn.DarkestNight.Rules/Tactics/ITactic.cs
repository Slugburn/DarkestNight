using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tactics
{
    public interface ITactic
    {
        string Name { get; }

        bool IsAvailable(Hero hero);

        int GetDiceCount();

        void AfterRoll(Hero hero, ICollection<int> roll);
    }
}

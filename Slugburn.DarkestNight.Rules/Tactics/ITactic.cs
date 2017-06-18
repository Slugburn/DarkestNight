using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tactics
{
    public interface ITactic
    {
        string Name { get; }
        void Use(Hero hero);

        bool IsAvailable(Hero hero);

        int GetDiceCount();
    }
}

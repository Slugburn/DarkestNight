using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tactics
{
    public interface ITactic
    {
        string Name { get; }
        TacticType Type { get; }
        void Use(Hero hero);

        bool IsAvailable(Hero hero);

        int GetDiceCount();
    }
}

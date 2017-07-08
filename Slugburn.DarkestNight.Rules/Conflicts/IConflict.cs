using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Conflicts
{
    public interface IConflict
    {
        string Name { get; }
        void Win(Hero hero);
        void Failure(Hero hero);
        string OutcomeDescription(bool isWin, TacticType tacticType);
    }
}
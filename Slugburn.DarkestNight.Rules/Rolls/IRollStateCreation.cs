using Slugburn.DarkestNight.Rules.Modifiers;

namespace Slugburn.DarkestNight.Rules.Rolls
{
    public interface IRollStateCreation
    {
        IRollStateCreation Type(ModifierType type);
        IRollStateCreation Base(string name, int diceCount);
        IRollStateCreation Target(int targetNumber);
    }
}
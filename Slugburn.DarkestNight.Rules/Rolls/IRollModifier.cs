using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Rolls
{
    public interface IRollModifier
    {
        int GetModifier(Hero hero, RollType rollType);
        string Name { get; }
    }
}

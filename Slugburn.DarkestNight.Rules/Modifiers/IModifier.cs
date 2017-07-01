using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Modifiers
{
    public interface IModifier
    {
        int GetModifier(Hero hero, ModifierType modifierType);
        string Name { get; }
    }
}

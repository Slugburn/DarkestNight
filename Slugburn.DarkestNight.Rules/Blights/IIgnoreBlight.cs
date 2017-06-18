using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public interface IIgnoreBlight
    {
        string Name { get; }
        bool IsIgnoring(Hero hero, Blight blight);
    }
}

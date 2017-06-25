using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules
{
    public interface IConflict
    {
        string Name { get; }
        void Win(Hero hero);
        void Failure(Hero hero);
    }
}
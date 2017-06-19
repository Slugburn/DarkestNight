using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public interface IActivateable
    {
        void Activate(Hero hero);
        bool Deactivate(Hero hero);
        bool IsActive { get; }
    }
}

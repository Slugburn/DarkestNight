using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules
{
    public interface ILocationSelectedHandler
    {
        void Handle(Hero hero, Location location);
    }
}

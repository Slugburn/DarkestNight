using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Rolls
{
    public interface IRollHandler
    {
        void HandleRoll(Hero hero);
    }
}
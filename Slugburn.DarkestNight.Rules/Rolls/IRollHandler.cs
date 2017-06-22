using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Rolls
{
    public interface IRollHandler
    {
        RollState HandleRoll(Hero hero, RollState rollState);
        void AcceptRoll(Hero hero, RollState rollState);
    }
}
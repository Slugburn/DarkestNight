using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    interface IOath : IPower, IActivateable
    {
        string FulfillText { get; }
        string BreakText { get; }
        void Fulfill(Hero hero);
        void Break(Hero hero);
    }
}
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public interface IPower : ISource
    {
        PowerType Type { get; }
        string Text { get; }
        bool StartingPower { get; }

        bool Exhausted { get; }
        bool IsUsable(Hero hero);

        void Learn(Hero hero);
        void Exhaust(Hero hero);
        void Refresh();
    }
}
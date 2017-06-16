namespace Slugburn.DarkestNight.Rules.Powers
{
    public interface IPower : ISource
    {
        PowerType Type { get; }
        string Text { get; }
        bool StartingPower { get; }

        bool Exhausted { get; }
        bool IsUsable();

        void Learn();
        void Exhaust();
        void Refresh();
    }
}
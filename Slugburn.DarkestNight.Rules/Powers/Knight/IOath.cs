namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    interface IOath : IPower, IActivateable
    {
        string FulfillText { get; }
        string BreakText { get; }
    }
}
using System;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public interface IPlayerExpectation : IThen
    {
        IPlayerEventExpectation EventView { get; }
        IPlayerExpectation SeesEvent(string title, string text, int fate, params string[] options);
        IPlayerExpectation SeesTarget(params string[] targetNames);
        IPlayerExpectation SeesTactics(params string[] tacticNames);
        IPlayerExpectation PowerSelectionView(params string[] powerNames);
        IPlayerExpectation Conflict(Action<PlayerConflictExpectation> expect);
        IPlayerExpectation BlightSelectionView(Action<PlayerBlightExpectation> expect);
        IPlayerExpectation LocationSelectionView(params string[] locations);
    }
}
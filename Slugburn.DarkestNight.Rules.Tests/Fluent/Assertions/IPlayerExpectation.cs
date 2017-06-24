using System;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public interface IPlayerExpectation : IThen
    {
        IPlayerEventExpectation Event { get; }
        IPlayerExpectation SeesEvent(string title, string text, int fate, params string[] options);
        IPlayerExpectation SeesTarget(params string[] targetNames);
        IPlayerExpectation SeesTactics(params string[] tacticNames);
        IPlayerExpectation Powers(params string[] powerNames);
        IPlayerExpectation Conflict(Action<PlayerConflictExpectation> expect);
        IPlayerExpectation Blights(Action<PlayerBlightExpectation> expect);
        IPlayerExpectation SelectingLocation(params string[] locations);
    }
}
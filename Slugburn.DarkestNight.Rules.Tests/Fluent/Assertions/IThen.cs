using System;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public interface IThen : ITestRoot
    {
        IPlayerExpectation Player { get; }
        IThen Hero(Action<HeroExpectation> expect);
        IGameExpectation Game { get; }
        IThen Location(string location, Action<LocationExpectation> expect);
    }
}
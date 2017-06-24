using System;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public interface IThen
    {
        IGiven Given { get; }
        IWhen When { get; }
        IThen Then { get; }

        IPlayerExpectation Player { get; }
        IThen Hero(Action<HeroExpectation> expect);
        IGameExpectation Game { get; }
        IThen Location(string location, Action<LocationExpectation> expect);
    }
}
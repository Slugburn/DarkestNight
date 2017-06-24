using System;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;
using IGiven = Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements.IGiven;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public interface IThen
    {
        IGiven Given { get; }
        IWhen When { get; }
        IThen Then { get; }

        IThen Player(Action<PlayerExpectation> expect);
        IThen Hero(Action<HeroExpectation> expect);
        IThen Game(Action<GameExpectation> expect);
        IThen Location(string location, Action<LocationExpectation> expect);
    }
}
using System;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public interface IGiven
    {
        IGiven Given { get; }
        IWhen When { get; }
        IThen Then { get; }

        IGiven Game(Action<GameContext> def);

        IGiven Location(string village, Action<LocationContext> def);
        IGiven ActingHero(Action<HeroContext> def = null);
        IGiven Configure(Func<IGiven, IGiven> setConditions);
    }
}
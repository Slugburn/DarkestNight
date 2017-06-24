using System;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public interface IWhen
    {
        IGiven Given { get; }
        IWhen When { get; }
        IThen Then { get; }

        IWhen Hero(Action<HeroActionContext> action);
        IWhen Player(Action<PlayerActionContext> action);
        IWhen Game(Action<GameActionContext> action);
    }
}
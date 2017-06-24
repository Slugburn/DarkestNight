using System;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public interface IWhen : ITestRoot
    {
        IHeroActionContext Hero { get; }
        IPlayerActionContext Player { get; }
        IWhen Game(Action<GameActionContext> action);
    }
}
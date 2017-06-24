using System;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public interface IWhen : ITestRoot
    {
        IWhen Hero(Action<IHeroActionContext> action);
        IPlayerActionContext Player();
        IWhen Game(Action<GameActionContext> action);
    }
}
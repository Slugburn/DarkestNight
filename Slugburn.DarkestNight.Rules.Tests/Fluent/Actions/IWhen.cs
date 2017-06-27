using System;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public interface IWhen : ITestRoot
    {
        IPlayerActionContext Player { get; }
        IGameActionContext Game { get; }
    }
}
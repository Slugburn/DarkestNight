using System;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public interface IHeroActionContext
    {
        IHeroActionContext DrawsEvent(string eventName = null);
        IHeroActionContext RefreshesPower(string powerName);
        IHeroActionContext MovesTo(string location);
        IHeroActionContext StartsTurn();
        IHeroActionContext Fights(Action<TacticContext> actions);
        IHeroActionContext Eludes(Action<TacticContext> actions);
        IHeroActionContext SelectsTactic(Action<TacticContext> define = null, string defaultTactic = "Fight");
    }
}
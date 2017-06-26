using System;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public interface IPlayerActionContext : IWhen
    {
        IPlayerActionContext AssignsDie(int dieValue, string target);
        IPlayerActionContext AcceptsConflictResults();
        IPlayerActionContext AcceptsNecromancerTurn();
        IPlayerActionContext AcceptsRoll();
        IPlayerActionContext ChoosesBlight(params string[] blights);
        IPlayerActionContext ChooseLocation(Location location);
        IPlayerActionContext ResolvesConflict(Action<ResolveConflictContext> action);
        IPlayerActionContext ResolvesConflict(IFakeContext fake);
        IPlayerActionContext SelectsBlight(string location, string blight);
        IPlayerActionContext SelectsEventOption(string option, IFakeContext set = null);
        IPlayerActionContext SelectsLocation(string location);
        IPlayerActionContext SelectsPower(string powerName);
        IPlayerActionContext Targets(params string[] targetNames);
        IPlayerActionContext UsesTactic(string tacticName);
        IPlayerActionContext TakesAction(string actionName);
    }
}
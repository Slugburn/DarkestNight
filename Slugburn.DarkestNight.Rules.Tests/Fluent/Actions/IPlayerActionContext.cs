using System;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public interface IPlayerActionContext : IWhen
    {
        IPlayerActionContext TakesAction(string actionName);
        IPlayerActionContext UsePower(string name, bool response = true);
        IPlayerActionContext ChoosesBlight(params string[] blights);
        IPlayerActionContext ChooseLocation(Location location);
        IPlayerActionContext SelectsEventOption(string option, IFakeRollContext set = null);
        IPlayerActionContext AcceptsRoll();
        IPlayerActionContext SelectsLocation(string location);
        IPlayerActionContext ResolvesConflict(Action<ResolveConflictContext> action);
        IPlayerActionContext SelectsPower(string powerName);
        IPlayerActionContext SelectsBlight(string location, string blight);
        IPlayerActionContext FinishNecromancerTurn();
        IPlayerActionContext AssignsDie(int dieValue, string target);
    }
}
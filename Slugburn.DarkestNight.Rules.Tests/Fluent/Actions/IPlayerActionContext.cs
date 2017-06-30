using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public interface IPlayerActionContext : IWhen
    {
        IPlayerActionContext AssignsDie(int dieValue, string target);
        IPlayerActionContext AcceptsConflictResults(int count = 1);
        IPlayerActionContext AcceptsNecromancerTurn();
        IPlayerActionContext AcceptsRoll();
        IPlayerActionContext ResolvesConflict(IFakeContext fake);
        IPlayerActionContext SelectsBlight(string location, string blightType);
        IPlayerActionContext SelectsEventOption(string option, IFakeContext set = null);
        IPlayerActionContext SelectsLocation(string location);
        IPlayerActionContext SelectsPower(string powerName);
        IPlayerActionContext Targets(params string[] targetNames);
        IPlayerActionContext UsesTactic(string tacticName);
        IPlayerActionContext TakesAction(string actionName, IFakeContext fake = null);
        IPlayerActionContext TakesAction(string heroName, string actionName, IFakeContext fake = null);
        IPlayerActionContext SelectsBlights(params string[] blights);
        IPlayerActionContext SelectsHero(string heroName);
        IPlayerActionContext AnswersQuestion(string title, bool answer);
        IPlayerActionContext SelectsSearchResult(string resultName = null);
    }
}
namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public interface IHeroActionContext : IWhen
    {
        IHeroActionContext RefreshesPower(string powerName);
        IHeroActionContext MovesTo(string location);
    }
}
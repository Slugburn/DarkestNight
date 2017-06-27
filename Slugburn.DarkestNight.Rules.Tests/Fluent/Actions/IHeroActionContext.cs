namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public interface IHeroActionContext : IWhen
    {
        IHeroActionContext DrawsEvent(string eventName = null);
        IHeroActionContext RefreshesPower(string powerName);
        IHeroActionContext MovesTo(string location);
        IHeroActionContext FacesEnemy(string enemyName);
    }
}
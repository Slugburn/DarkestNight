namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public interface IHeroContext : IGameContext
    {
        IHeroContext HasPowers(params string[] names);
        IHeroContext At(string location);
        IHeroContext NotAt(string location);
        IHeroContext Secrecy(int value);
        IHeroContext Grace(int value);
        IHeroContext LostGrace(int amount);
        IHeroContext PowerDeck(params string[] powers);

        IPowerContext Power(string powerName);
        IHeroContext HasDrawnEvent(string eventName = null);
        IHeroContext FacesEnemy(string enemyName);
        IHeroContext RefreshesPower(string powerName);
        IHeroContext MovesTo(string location);
        IHeroContext IsTakingTurn(bool isTakingTurn = true);
        IHeroContext HasTakenAction(bool hasTakenaction = true);
        IHeroContext HasItems(params string[] itemNames);
        IHeroContext NextPowerDraws(params string[] powerNames);
        IHeroContext LostSecrecy(int amount);
        IHeroContext DefaultGrace(int defaultGrace);
    }
}
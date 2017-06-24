namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public interface IHeroContext : IGameContext
    {
        IHeroContext HasPowers(params string[] names);
        IHeroContext At(string location);
        IHeroContext Secrecy(int value);
        IHeroContext Grace(int value);
        IHeroContext PowerDeck(params string[] powers);
    }
}
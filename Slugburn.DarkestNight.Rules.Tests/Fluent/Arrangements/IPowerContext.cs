namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public interface IPowerContext : IHeroContext
    {
        IPowerContext IsActive(string target = null);
        IPowerContext IsExhausted(bool exhausted = true);
    }
}
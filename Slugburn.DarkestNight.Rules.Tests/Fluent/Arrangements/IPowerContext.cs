namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public interface IPowerContext : IHeroContext
    {
        IPowerContext IsActive();
        IPowerContext IsExhausted(bool exhausted = true);
    }
}
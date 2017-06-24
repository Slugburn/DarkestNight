namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public interface IPowerContext : IGiven
    {
        IPowerContext IsActive();
        IPowerContext IsExhausted();
    }
}
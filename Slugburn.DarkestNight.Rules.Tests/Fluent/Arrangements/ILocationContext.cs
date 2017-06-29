namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public interface ILocationContext : IGameContext
    {
        ILocationContext Blights(params string[] blights);
        ILocationContext HasRelic(bool hasRelic = true);
    }
}
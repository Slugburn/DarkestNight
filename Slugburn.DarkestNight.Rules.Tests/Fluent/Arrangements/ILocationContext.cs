namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public interface ILocationContext : IGameContext
    {
        ILocationContext HasBlights(params string[] blights);
        ILocationContext HasRelic(bool hasRelic = true);
    }
}
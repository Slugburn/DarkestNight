using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules
{
    public interface IMap
    {
        BlightType GetBlightType(Location location);
        SearchResult GetSearchResult(Location location);
    }
}
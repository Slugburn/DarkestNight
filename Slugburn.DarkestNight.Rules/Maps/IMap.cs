using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Maps
{
    public interface IMap
    {
        BlightType GetBlight(Location location);
        Find GetSearchResult(Location location);
    }
}
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Maps
{
    public interface IMap
    {
        Blight GetBlight(Location location);
        Find GetSearchResult(Location location);
    }
}
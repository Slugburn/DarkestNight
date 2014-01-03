using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules
{
    public interface IMap
    {
        Blight GetBlight(Location location);
        Find GetSearchResult(Location location);
    }
}
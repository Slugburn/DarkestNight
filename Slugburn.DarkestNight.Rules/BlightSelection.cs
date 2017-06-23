using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules
{
    public class BlightSelection
    {
        public Location Location { get; set; }
        public Blight Blight { get; set; }

        public BlightSelection(Location location, Blight blight)
        {
            Location = location;
            Blight = blight;
        }
    }
}
namespace Slugburn.DarkestNight.Rules.Blights
{
    public class BlightLocation
    {
        public BlightLocation(Blight blight, Location location)
        {
            Location = location;
            Blight = blight;
        }

        public Blight Blight { get; set; }
        public Location Location { get; set; }
    }
}
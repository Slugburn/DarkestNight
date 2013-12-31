namespace Slugburn.DarkestNight.Rules.Spaces
{
    class Monastery : Space
    {
        public Monastery()
        {
            Location = Location.Monastery;
            Name = "Monastery";
            AdjacentLocations = new[] {Location.Mountains, Location.Village, Location.Forest};
        }
    }
}

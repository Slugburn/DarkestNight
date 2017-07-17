using Slugburn.DarkestNight.Rules.Spaces;

namespace Slugburn.DarkestNight.Rules.IO
{
    public class SpaceData
    {
        public static SpaceData Create(Space space)
        {
            var data = new SpaceData
            {
                Location = space.Location,
                HasRelic = space.HasRelic
            };
            return data;
        }

        public Location Location { get; set; }
        public bool HasRelic { get; set; }

        public void Restore(Game game)
        {
            var space = game.Board[Location];
            space.HasRelic = HasRelic;
        }
    }
}
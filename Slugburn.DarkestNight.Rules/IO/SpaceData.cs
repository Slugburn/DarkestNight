using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
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
                Blights = space.Blights.Select(x => x.Type).ToList(),
                HasRelic = space.HasRelic
            };
            return data;
        }

        public Location Location { get; set; }
        public List<BlightType> Blights { get; set; }
        public bool HasRelic { get; set; }

        public void Restore(Game game)
        {
            var space = game.Board[Location];
            space.HasRelic = HasRelic;
            foreach (var blightType in Blights)
                game.CreateBlight(Location, blightType);
        }
    }
}
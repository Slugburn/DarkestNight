using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Spaces;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class LocationModel
    {
        public LocationModel(Game game, Space space)
        {
            Name = space.Location.ToString();
            HasRelic = space.HasRelic;
            SearchTarget = space.GetSearchTarget(game.ActingHero);
            Tokens = CreateTokens(game, space.Location);
            Blights = BlightModel.Create(space.Blights);
        }

        public string Name { get; set; }
        public bool HasRelic { get; set; }
        public List<string> Tokens { get; set; }
        public int? SearchTarget { get; set; }
        public List<BlightModel> Blights { get; set; }

        private List<string> CreateTokens(Game game, Location location)
        {
            var list = game.Heroes.Where(h => h.Location == location).Select(h => h.Name).ToList();
            if (game.Necromancer.Location == location)
                list.Insert(0, "Necromancer");
            return list;
        }
    }
}
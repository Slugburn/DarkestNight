using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Spaces;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class LocationModel
    {
        public LocationModel(Space space)
        {
            var game = space.Game;
            Name = space.Location.ToString();
            HasRelic = space.HasRelic;
            SearchTarget = space.GetSearchTarget(game.ActingHero);
            Blights = BlightModel.Create(space.Blights);
            IsNecromancerHere = game.Necromancer.Location == space.Location;
            Effects = EffectModel.Create(space.GetEffects());
        }

        public string Name { get; set; }
        public bool HasRelic { get; set; }
        public int? SearchTarget { get; set; }
        public List<BlightModel> Blights { get; set; }
        public bool IsNecromancerHere { get; set; }
        public List<EffectModel> Effects { get; set; }
    }
}
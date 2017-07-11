using Slugburn.DarkestNight.Rules.Enemies;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class NecromancerModel
    {
        public int Roll { get; set; }
        public Location Location { get; set; }
        public Location RolledDestination { get; set; }
        public Location Destination { get; set; }
        public string DetectedHero { get; set; }
        public Callback<object> Callback { get; set; }

        public static NecromancerModel Create(Necromancer necromancer)
        {
            return new NecromancerModel
            {
                Location = necromancer.Location,
                Roll = necromancer.MovementRoll,
                RolledDestination = necromancer.RolledDestination,
                Destination = necromancer.Destination,
                DetectedHero = necromancer.DetectedHero?.Name,
                Callback = Players.Callback.For(null, necromancer)
            };
        }

        
        public string Description
        {
            get
            {
                if (Destination == RolledDestination)
                    return Destination == Location
                        ? $"The Necromancer is staying at the {Location}."
                        : $"The Necromancer is moving to the {Destination}.";
                return Destination == Location
                    ? $"The Necromancer detected the {DetectedHero}, so he is staying at the {Destination} instead of moving to the {RolledDestination}."
                    : $"The Necromancer detected the {DetectedHero}, so he is moving to the {Destination} instead of the {RolledDestination}.";
            }
        }
    }
}

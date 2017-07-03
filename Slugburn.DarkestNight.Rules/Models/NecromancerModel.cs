using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Enemies;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class NecromancerModel
    {
        public NecromancerModel(int roll, Location movingTo, List<string> detected)
        {
            Roll = roll;
            MovingTo = movingTo;
            Detected = detected;
        }

        public int Roll { get; set; }

        public Location MovingTo { get; set; }

        public ICollection<string> Detected { get; }

        public static NecromancerModel From(Necromancer necromancer)
        {
            return new NecromancerModel(necromancer.MovementRoll, necromancer.Destination, necromancer.DetectedHeroes.Select(x => x.Name).ToList());
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Enemies;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerNecromancer
    {
        public PlayerNecromancer(int roll, Location movingTo, List<string> detected)
        {
            Roll = roll;
            MovingTo = movingTo;
            Detected = detected;
        }

        public int Roll { get; set; }

        public Location MovingTo { get; set; }

        public ICollection<string> Detected { get; }

        public static PlayerNecromancer From(Necromancer necromancer)
        {
            return new PlayerNecromancer(necromancer.MovementRoll, necromancer.Destination, necromancer.DetectedHeroes.Select(x => x.Name).ToList());
        }
    }
}

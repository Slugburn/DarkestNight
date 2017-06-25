using System.Collections.Generic;

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
    }
}

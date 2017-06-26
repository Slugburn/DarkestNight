using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerBlightSelection
    {
        public PlayerBlightSelection(ICollection<PlayerBlight> blights, int max = 1)
        {
            Blights = blights;
            Max = max;
        }

        public ICollection<PlayerBlight> Blights { get; set; }
        public int Max { get; set; }
    }
}

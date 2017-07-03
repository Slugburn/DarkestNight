using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class BlightSelectionModel
    {
        public BlightSelectionModel(ICollection<BlightModel> blights, int max = 1)
        {
            Blights = blights;
            Max = max;
        }

        public ICollection<BlightModel> Blights { get; set; }
        public int Max { get; set; }
    }
}

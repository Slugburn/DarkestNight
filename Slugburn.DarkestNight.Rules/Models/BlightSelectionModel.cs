using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class BlightSelectionModel
    {
        public BlightSelectionModel(string title, ICollection<BlightModel> blights, int max)
        {
            Title = title;
            Blights = blights;
            Max = max;
        }

        public string Title { get; }
        public ICollection<BlightModel> Blights { get; }
        public int Max { get; }

        public static BlightSelectionModel Create(string title, IEnumerable<IBlight> blights, int max)
        {
            var blightModels = BlightModel.Create(blights);
            var selectionModel = new BlightSelectionModel(title, blightModels, max);
            return selectionModel;
        }
    }
}
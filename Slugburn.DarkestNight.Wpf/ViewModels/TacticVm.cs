using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class TacticVm
    {
        public TacticVm(TacticModel m)
        {
            Name = m.Name;
            Type = m.Type;
            Dice = $"{m.DiceCount}d";
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Dice { get; set; }

        public static List<TacticVm> CreateTactics(ConflictModel model)
        {
            return model.Tactics?.Select(m => new TacticVm(m)).ToList();
        }
    }
}
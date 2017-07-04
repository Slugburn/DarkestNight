using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class Tactic
    {
        public Tactic(TacticModel m)
        {
            Name = m.Name;
            Type = m.Type;
            Dice = $"{m.DiceCount}d";
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Dice { get; set; }
    }
}
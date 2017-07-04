using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class TacticModel
    {
        public TacticModel(TacticInfo info)
        {
            Name = info.Name;
            Type = info.Type.ToString();
            DiceCount = info.DiceCount;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public int DiceCount { get; set; }
    }
}
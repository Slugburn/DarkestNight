using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tactics
{
    public class TacticInfo
    {
        public string Name { get; set; }
        public TacticType Type { get; set; }
        public int DiceCount { get; set; }
        public List<DiceDetail> DiceDetails { get; set; }
    }
}
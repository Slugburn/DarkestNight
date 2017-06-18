using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Heroes.Impl;

namespace Slugburn.DarkestNight.Rules
{
    public class TacticInfo
    {
        public string Name { get; set; }
        public int DiceCount { get; set; }
        public List<DiceDetail> DiceDetails { get; set; }
    }
}
using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes.Impl;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules
{
    public class ConflictState
    {
        public TacticType TacticType { get; set; }
        public int MinTarget { get; set; }
        public int MaxTarget { get; set; }
        public ICollection<Blight> Targets { get; set; }
        public TacticInfo SelectedTactic { get; set; }
        public List<TacticInfo> AvailableFightTactics { get; set; }
        public ICollection<Blight> AvailableTargets { get; set; }
        public List<int> Roll { get; set; }
    }
}
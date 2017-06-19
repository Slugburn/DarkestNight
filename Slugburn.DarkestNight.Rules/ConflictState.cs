using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules
{
    public class ConflictState
    {
        public ConflictType ConflictType { get; set; }
        public int MinTarget { get; set; }
        public int MaxTarget { get; set; }
        public ICollection<TargetInfo> SelectedTargets { get; set; }
        public TacticInfo SelectedTactic { get; set; }
        public List<TacticInfo> AvailableTactics { get; set; }
        public List<TacticInfo> AvailableEvadeTactics { get; set; }
        public ICollection<TargetInfo> AvailableTargets { get; set; }
    }
}
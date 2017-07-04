using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class ConflictModel
    {
        public List<TargetModel> Targets { get; set; }
        public List<TacticModel> Tactics { get; set; }
        public int TargetCount { get; set; }
        public ICollection<int> Roll { get; set; }
        public bool IsAccepted { get; set; }

        public ConflictEffect Effect { get; set; }
        public bool? Win { get; set; }

        public bool IsRollAccepted { get; set; }

        public static ConflictModel FromConflictState(ConflictState state)
        {
            var targets = state.SelectedTargets?.Select(CreateTarget).ToList() ?? state.AvailableTargets.Select(CreateTarget).ToList();
            var tactics = state.SelectedTactic == null
                ? state.AvailableTactics.Select(CreateTactic).ToList()
                : new List<TacticModel> {CreateTactic(state.SelectedTactic)};
            var targetCount = state.MaxTarget;
            var roll = state.Roll ?? new List<int>();
            bool? win = null;
            var resolvedTargets = state.SelectedTargets?.Where(x => x.ResultNumber != null).ToList();
            if (resolvedTargets != null && resolvedTargets.Any())
                win = resolvedTargets.Any(x => x.IsWin);
            var isRollAccepted = state.IsRollAccepted;
            return new ConflictModel {Targets = targets, Tactics = tactics, TargetCount = targetCount, Roll = roll, Win = win, IsRollAccepted = isRollAccepted};
        }

        private static TacticModel CreateTactic(TacticInfo info)
        {
            return new TacticModel(info);
        }

        private static TargetModel CreateTarget(TargetInfo targetInfo)
        {
            return new TargetModel(targetInfo);
        }

        public static TargetModel CreateTarget(ConflictTarget target)
        {
            return new TargetModel(target);
        }

        public class ConflictEffect
        {
            public int TargetName { get; set; }
            public int Roll { get; set; }
            public bool Success { get; set; }
            public string Outcome { get; set; }
            public string Effect { get; set; }
        }
    }
}
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
        public List<ConflictTargetModel> SelectedTargets { get; set; }
        public ICollection<int> Roll { get; set; }
        public bool IsAccepted { get; set; }

        public ConflictEffect Effect { get; set; }

        public bool IsRollAccepted { get; set; }

        public static ConflictModel FromConflictState(ConflictState state)
        {
            var isRollAccepted = state.IsRollAccepted;
            var model = new ConflictModel {Roll = state.Roll, IsRollAccepted = isRollAccepted};
            if (state.Roll == null)
            {
                model.TargetCount = state.MaxTarget;
                model.Targets = state.AvailableTargets.Select(CreateTarget).ToList();
                model.Tactics = state.AvailableTactics.Select(CreateTactic).ToList();
            }
            else
            {
                model.SelectedTargets = state.SelectedTargets?.Select(CreateTarget).ToList();
                model.SelectedTactic = CreateTactic(state.SelectedTactic);
            }
            return model;
        }

        public TacticModel SelectedTactic { get; set; }

        private static TacticModel CreateTactic(TacticInfo info)
        {
            return new TacticModel(info);
        }

        private static TargetModel CreateTarget(TargetInfo targetInfo)
        {
            return new TargetModel(targetInfo);
        }

        public static ConflictTargetModel CreateTarget(ConflictTarget conflictTarget)
        {
            return new ConflictTargetModel(conflictTarget);
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
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerConflict
    {
        public List<Target> Targets { get; set; }
        public List<Tactic> Tactics { get; set; }
        public int TargetCount { get; set; }
        public ICollection<int> Roll { get; set; }

        public ConflictEffect Effect { get; set; }

        public class Tactic
        {
            public string Name { get; set; }
        }

        public class Target
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class ConflictEffect
        {
            public int TargetName { get; set; }
            public int Roll { get; set; }
            public bool Success { get; set; }
            public string Outcome { get; set; }
            public string Effect { get; set; }
        }

        public static PlayerConflict FromConflictState(ConflictState state)
        {
            var targets = state.SelectedTargets?.Select(CreateTarget).ToList() ?? state.AvailableTargets.Select(CreateTarget).ToList();
            var tactics = state.SelectedTactic == null 
                ? state.AvailableTactics.Select(CreateTactic).ToList() 
                : new List<Tactic> {CreateTactic(state.SelectedTactic)};
            var targetCount = state.MaxTarget;
            var roll = state.Roll ?? new List<int>();
            return new PlayerConflict {Targets = targets, Tactics = tactics, TargetCount = targetCount, Roll = roll};
        }

        private static Tactic CreateTactic(TacticInfo x)
        {
            return new Tactic {Name = x.Name};
        }

        private static Target CreateTarget(TargetInfo targetInfo)
        {
            return new Target {Id = targetInfo.Id, Name = targetInfo.Name};
        }

        public static Target CreateTarget(ConflictTarget target)
        {
            return new Target() {Id = target.Id, Name = target.Name};
        }
    }

}

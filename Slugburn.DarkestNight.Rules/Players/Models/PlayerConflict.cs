using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerConflict
    {
        public List<Target> Targets { get; set; }
        public List<Tactic> Tactics { get; set; }
        public int TargetCount { get; set; }
        public ICollection<int> Roll { get; set; }

        public class Tactic
        {
            public string Name { get; set; }
        }

        public class Target
        {
            public int Id { get; set; }
            public string Name { get; set; }
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

        private static Target CreateTarget(TargetInfo x)
        {
            return new Target {Id = x.Id, Name = x.Name};
        }
    }
}

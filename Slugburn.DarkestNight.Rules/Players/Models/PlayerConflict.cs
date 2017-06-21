using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Enemies;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerConflict
    {
        public List<Target> Targets { get; set; }
        public List<Tactic> Tactics { get; set; }

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
            var targets = state.AvailableTargets.Select(x=>new Target {Id = x.Id, Name = x.Name}).ToList();
            var tactics = state.AvailableTactics.Select(x => new Tactic {Name = x.Name}).ToList();
            return new PlayerConflict {Targets = targets, Tactics = tactics};
        }
    }
}

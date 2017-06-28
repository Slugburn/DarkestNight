using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Conflicts
{
    public class TargetInfo
    {
        public TargetInfo(IConflict conflict)
        {
            Conflict = conflict;
            Name = conflict.Name;
        }

        public int Id { get; set; }
        public string Name { get; }
        public bool CanFight { get; set; }
        public bool CanElude { get; set; }
        public int FightTarget { get; set; }
        public int EludeTarget { get; set; }
        public List<ConflictResult> Results { get; set; }
        public IConflict Conflict { get; set; }
    }

    public class ConflictResult
    {
        public ConflictResult(string outcome, string effect)
        {
            Outcome = outcome;
            Effect = effect;
        }

        public string Outcome { get; set; }
        public string Effect { get; set; }
    }
}

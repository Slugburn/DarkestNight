using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slugburn.DarkestNight.Rules
{
    public class TargetInfo
    {
        public string Name { get; set; }
        public bool CanFight { get; set; }
        public bool CanElude { get; set; }
        public int FightTarget { get; set; }
        public int EludeTarget { get; set; }
        public List<ConflictResult> Results { get; set; }
        public int Id { get; set; }
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

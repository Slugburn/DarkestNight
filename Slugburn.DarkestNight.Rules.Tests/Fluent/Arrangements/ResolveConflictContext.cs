using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class ResolveConflictContext : IFakeContext
    {
        private readonly PlayerConflict _conflict;
        private string _tactic;
        private List<int> _targetIds;

        public ResolveConflictContext(PlayerConflict conflict)
        {
            _conflict = conflict;
        }

        public ResolveConflictContext Tactic(string name)
        {
            _tactic = name;
            return this;
        }

        public ResolveConflictContext Target(params string[] targets)
        {
            _targetIds = _conflict.Targets.Where(x => targets.Contains(x.Name)).Select(x => x.Id).ToList();
            return this;
        }

        public string GetTactic() => _tactic;

        public ICollection<int> GetTargetIds() => _targetIds;
    }
}
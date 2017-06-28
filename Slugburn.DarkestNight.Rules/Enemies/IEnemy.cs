using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Conflicts;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public interface IEnemy : IConflict
    {
        int Fight { get; }
        int Elude { get; }
        IEnumerable<ConflictResult> GetResults();
    }
}

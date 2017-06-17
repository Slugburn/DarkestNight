using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules
{
    public interface IRollClient
    {
        void EndCombat(IEnumerable<int> roll);
    }
}
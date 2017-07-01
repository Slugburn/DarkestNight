using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Modifiers
{
    public class ModifierSummary
    {
        internal ModifierSummary(List<ModifierDetail> details, int total)
        {
            Details = details;
            Total = total;
        }

        public List<ModifierDetail> Details { get; }

        public int Total { get; }
    }
}
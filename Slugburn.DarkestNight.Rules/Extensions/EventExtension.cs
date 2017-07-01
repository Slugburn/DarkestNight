using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Events;

namespace Slugburn.DarkestNight.Rules
{
    public static class EventExtension
    {
        public static bool CanIgnore(this IEventCard card)
        {
            var canIgnore = card.Detail.Name != "Renewal";
            return canIgnore;
        }

        public static void Activate(this IEnumerable<HeroEventRow> rows, int result)
        {
            foreach (var row in rows.Where(x => x.Min <= result && result <= x.Max))
                row.IsActive = row.Min <= result && result <= row.Max;
        }
    }
}

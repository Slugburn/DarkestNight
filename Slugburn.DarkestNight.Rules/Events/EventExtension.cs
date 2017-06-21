using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events
{
    public static class EventExtension
    {
        public static bool CanIgnore(this IEventCard card)
        {
            var canIgnore = card.Name != "Renewal";
            return canIgnore;
        }

        public static HeroEvent GetHeroEvent(this IEventCard card, Hero hero)
        {
            return new HeroEvent
            {
                Name = card.Name,
                Title = card.Name,
                Fate = card.Fate,
                Text = card.Detail.GetText(),
                Rows = card.Detail.GetRows(),
                Options = card.Detail.GetOptions(hero),
                IsIgnorable = card.CanIgnore()
            };
        }

        public static void Activate(this IEnumerable<EventRow> rows, int result)
        {
            foreach (var row in rows.Where(x => x.Min <= result && result <= x.Max))
                row.IsActive = row.Min <= result && result <= row.Max;
        }
    }
}

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
                Options = card.Detail.GetOptions(hero),
                IsIgnorable = card.CanIgnore()
            };
        }
    }
}

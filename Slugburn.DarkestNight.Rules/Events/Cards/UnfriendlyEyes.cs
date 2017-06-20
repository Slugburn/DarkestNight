using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class UnfriendlyEyes : IEventCard
    {
        public string Name => "Unfriendly Eyes";
        public int Fate { get; }

        public EventDetail Detail => EventDetail.Create(x => x
            .Text("Count the blights in your location", "0: Lose 1 Secrecy", "1-2: Spend 1 Secrecy or Lose 1 Grace", "3-4: Spend 1 Grace or +1 Darkness"));

        public void Resolve(Hero hero, string option)
        {
            var count= hero.GetBlights().Count;
            if (count == 0)
            {
                hero.LoseSecrecy("Event");
                return;
            }
            var e = count < 3
                ? new EventCard(Name, x => x
                    .Text("Spend 1 Secrecy or Lose 1 Grace")
                    .Option("secrecy", "Spend Secrecy", h => h.Secrecy > 0)
                    .Option("grace", "Lose Grace"), SecrecyOrGrace)
                : new EventCard(Name, x => x
                    .Text("Spend 1 Grace or +1 Darkness")
                    .Option("grace", "Spend Grace", h => h.Grace > 0)
                    .Option("darkness", "+1 Darkness"), GraceOrDarkness);
            hero.PresentCurrentEvent();
        }

        private void SecrecyOrGrace(Hero hero, string option)
        {
            if (option=="secrecy")
                hero.SpendSecrecy(1);
            else if (option == "grace")
                hero.LoseGrace();
            else
                throw new ArgumentOutOfRangeException(nameof(option));
        }

        private void GraceOrDarkness(Hero hero, string option)
        {
            if (option == "grace")
                hero.SpendGrace(1);
            else if (option == "darkness")
                hero.Game.IncreaseDarkness();
            else
                throw new ArgumentOutOfRangeException(nameof(option));
        }
    }
}
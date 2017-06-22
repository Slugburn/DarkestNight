using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class EvilDay : IEventCard
    {
        public EventDetail Detail => EventDetail.Create("Evil Day", 0,
            x => x
                .Text("Exhaust a power or draw 2 more events.")
                .Option("exhaust", "Exhaust a power", hero => hero.Powers.Any(power => !power.Exhausted))
                .Option("draw", "Draw 2 events"));

        public void Resolve(Hero hero, string option)
        {
            if (option == "exhaust")
                throw new NotImplementedException();
            else if (option == "draw")
                throw new NotImplementedException();
            else
                throw new ArgumentOutOfRangeException(nameof(option));
        }
    }
}

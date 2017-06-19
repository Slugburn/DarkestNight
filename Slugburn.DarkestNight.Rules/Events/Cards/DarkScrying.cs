using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class DarkScrying : IEventCard
    {
        public string Name => "Dark Scrying";

        public EventDetail Detail => EventDetail.Create(x =>
            x.Text("Spend 1 Grace or lose 2 Secrecy.")
                .Option("grace", "Spend 1 Grace", hero => hero.Grace > 0)
                .Option("secrecy", "Lose 2 Secrecy"));

        public void Resolve(Hero hero, string option)
        {
            if (option=="grace")
                hero.SpendGrace(1);
            else if (option == "secrecy")
                hero.LoseSecrecy(2, "Event");
            else
                throw new ArgumentOutOfRangeException(nameof(option));
        }
    }
}

using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class DarkScrying : IEventCard
    {
        public EventDetail Detail => EventDetail.Create("Dark Scrying", 4, x =>
            x.Text("Spend 1 Grace or lose 2 Secrecy.")
                .Option("spend-grace", "Spend Grace", hero => hero.CanSpendGrace)
                .Option("lose-secrecy", "Lose Secrecy"));

        public void Resolve(Hero hero, string option)
        {
            switch (option)
            {
                case "spend-grace":
                    hero.SpendGrace(1);
                    break;
                case "lose-secrecy":
                    hero.LoseSecrecy(2, "Event");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(option));
            }
            hero.EndEvent();
        }
    }
}

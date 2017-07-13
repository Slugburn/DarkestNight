using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class SloppySearch : IEventCard
    {
        public EventDetail Detail => EventDetail.Create("Sloppy Search", 2,
            x => x
                .Text("Roll 1d and take the highest")
                .Row(6, "Gain 1 Secrecy", o => o.Option("gain-secrecy", "Gain Secrecy"))
                .Row(4, 5, "No effect", o => o.Option("no-effect", "No Effect"))
                .Row(1, 3, "Spend 1 Grace or lose 1 Secrecy", o => o
                    .Option("spend-grace", "Spend Grace", h => h.CanSpendGrace)
                    .Option("lose-secrecy", "Lose Secrecy"))
                .Option("roll", "Roll"));

        public void Resolve(Hero hero, string option)
        {
            switch (option)
            {
                case "roll":
                    hero.RollEventDice(new EventRollHandler(Detail));
                    return;
                case "gain-secrecy":
                    hero.GainSecrecy(1, int.MaxValue);
                    break;
                case "no-effect":
                    // no effect
                    break;
                case "spend-grace":
                    hero.SpendGrace(1);
                    break;
                case "lose-secrecy":
                    hero.LoseSecrecy(1, "Event");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option);
            }
            hero.EndEvent();
        }
    }
}

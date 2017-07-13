using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class CloseCall : IEventCard
    {
        public EventDetail Detail { get; } = 
            EventDetail.Create("Close Call", 4,
            x => x.Text("Roll 1d and take the highest")
                .Row(5, 6, "No effect", o => o.Option("no-effect", "Continue"))
                .Row(3, 4, "Lose 1 Secrecy", o => o.Option("lose-secrecy", "Continue"))
                .Row(1, 2, "Lose 1 Grace", o => o.Option("lose-grace", "Continue"))
                .Option("roll", "Roll"));

        public void Resolve(Hero hero, string option)
        {
            switch (option)
            {
                case "roll":
                    hero.RollEventDice(new EventRollHandler(Detail));
                    return;
                case "no-effect":
                    break;
                case "lose-secrecy":
                    hero.LoseSecrecy(1, "Event");
                    break;
                case "lose-grace":
                    hero.LoseGrace(1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option);
            }
            hero.EndEvent();
        }
    }
}

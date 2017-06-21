using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class SloppySearch : IEventCard
    {
        public string Name => "Sloppy Search";
        public int Fate { get; }

        public EventDetail Detail => EventDetail.Create(x => x
            .Text("Roll 1 die and take the highest")
            .Row(6, "Gain 1 Secrecy").Row(4, 5, "No effect").Row(1, 3, "Spend 1 Grace or lose 1 Secrecy")
            .Option("cont", "Continue"));

        public void Resolve(Hero hero, string option)
        {
            hero.RollEventDice(new SloppySearchRollHandler());
        }

        public class SloppySearchRollHandler : IRollHandler
        {
            public void HandleRoll(Hero hero)
            {
                var roll = hero.Roll;
                var result = roll.Max();
                if (result == 6)
                    hero.GainSecrecy(1, int.MaxValue);
                if (result < 4)
                {
                    var e = new EventCard("Sloppy Search", 0, x =>
                        x.Text("Spend 1 Grace or lose 1 Secrecy")
                            .Option("grace", "Spend Grace", h => h.Grace > 0)
                            .Option("secrecy", "Lose Secrecy"),
                        HandleEvent);
                    hero.PresentCurrentEvent();
                }
            }

            private void HandleEvent(Hero hero, string option)
            {
                switch (option)
                {
                    case "grace":
                        hero.SpendGrace(1);
                        break;
                    case "secrecy":
                        hero.LoseSecrecy("Event");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(option));
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class CloseCall : IEventCard
    {
        public string Name => "Close Call";
        public int Fate => 4;

        public EventDetail Detail { get; } = EventDetail.Create(x => x.Text("Roll 1d and take the highest")
            .Row(5, 6, "No effect")
            .Row(3, 4, "Lose 1 Secrecy")
            .Row(1, 2, "Lose 1 Grace")
            .Option("roll", "Roll"));

        public void Resolve(Hero hero, string option)
        {
            switch (option)
            {
                case "roll":
                    var dice = hero.GetDice(RollType.Event, "Event", 1);
                    hero.Roll = Die.Roll(dice.Total);
                    hero.SetRollHandler(new CloseCallRollHandler());
                    hero.State = HeroState.RollAvailable;
                    break;
                case "cont":
                    hero.EndEvent();
                    break;
            }
        }

        public class CloseCallRollHandler : IRollHandler
        {
            public void HandleRoll(Hero hero)
            {
                hero.RemoveRollHandler(this);
                var e = hero.CurrentEvent;
                var result = hero.Roll.Max();
                if (result == 5 || result == 6)
                {
                    e.Text = "No effect";
                }
                else if (result == 3 || result == 4)
                {
                    hero.LoseSecrecy("Event");
                    e.Text = "Lose 1 Secrecy";
                }
                else if (result == 1 || result == 2)
                {
                    hero.LoseGrace();
                    e.Text = "Lose 1 Grace";
                }
                else
                {
                    throw new InvalidOperationException("Unexpected roll.");
                }
                e.Options = new List<EventOption> { EventOption.Continue() };
                hero.PresentCurrentEvent();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class CloseCall : IEventCard
    {
        public EventDetail Detail { get; } = EventDetail.Create("Close Call", 4,
            x => x.Text("Roll 1d and take the highest")
                .Row(5, 6, "No effect")
                .Row(3, 4, "Lose 1 Secrecy")
                .Row(1, 2, "Lose 1 Grace")
                .Option("roll", "Roll"));

        public void Resolve(Hero hero, string option)
        {
            switch (option)
            {
                case "roll":
                    hero.RollEventDice(new CloseCallRollHandler());
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
                var result = hero.Roll.Result;
                if (result == 5 || result == 6)
                {
                }
                else if (result == 3 || result == 4)
                {
                    hero.LoseSecrecy("Event");
                }
                else if (result == 1 || result == 2)
                {
                    hero.LoseGrace();
                }
                else
                {
                    throw new InvalidOperationException("Unexpected roll.");
                }
                e.Options = new List<HeroEventOption> { HeroEventOption.Continue() };
                hero.PresentCurrentEvent();
            }

            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                var e = hero.CurrentEvent;
                e.Rows.Activate(rollState.Result);
                e.Options = new List<HeroEventOption> { HeroEventOption.Continue() };
                hero.PresentCurrentEvent();
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                hero.RemoveRollHandler(this);
                var result = rollState.Result;
                 if (result == 3 || result == 4)
                {
                    hero.LoseSecrecy("Event");
                }
                else if (result == 1 || result == 2)
                {
                    hero.LoseGrace();
                }
                 hero.EndEvent();
            }
        }
    }
}

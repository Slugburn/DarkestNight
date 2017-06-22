using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class LatentSpell : IEventCard
    {
        public EventDetail Detail => EventDetail.Create("Latent Spell",0, x => x
            .Text("Lose 1 Secrecy. Then, spend 1 Grace or discard this event without further effect.\nRoll 1d and take the highest")
            .Row(6, "Destroy a blight of your choice anywhere on the board")
            .Row(5, "Draw a power card")
            .Row(4, "Move to any other location")
            .Row(1, 3, "No effect")
            .Option("grace", "Spend Grace", hero => hero.Grace > 0)
            .Option("discard", "Discard Event"));

        public void Resolve(Hero hero, string option)
        {
            switch (option)
            {
                case "discard":
                    hero.EndEvent();
                    break;
                case "grace":
                    hero.SpendGrace(1);
                    hero.RollEventDice(new LatentSpellRollHandler());
                    break;
                case "cont":
                    hero.AcceptRoll();
                    break;
            }
        }

        public class LatentSpellRollHandler : IRollHandler
        {
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
                var result = rollState.Result;
                if (result == 6)
                {
                    // destroy a blight of your choice anywhere on the board
                    throw new NotImplementedException();
                }
                else if (result == 5)
                {
                    var powerName = hero.PowerDeck.First().Name;
                    hero.LearnPower(powerName);
                }
                else if (result == 4)
                {
                    // move to any other location
                    throw new NotImplementedException();
                }
                else
                {
                    // no effect
                }
                hero.EndEvent();
            }
        }
    }
}

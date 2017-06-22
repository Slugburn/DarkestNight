using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class SloppySearch : IEventCard
    {
        public EventDetail Detail => EventDetail.Create("Sloppy Search", 0, 
            x => x
            .Text("Roll 1 die and take the highest")
            .Row(6, "Gain 1 Secrecy")
            .Row(4, 5, "No effect")
            .Row(1, 3, "Spend 1 Grace or lose 1 Secrecy")
            .Option("roll", "Roll"));

        public void Resolve(Hero hero, string option)
        {
            if (option == "roll")
            {
                hero.RollEventDice(new SloppySearchRollHandler());
                return;
            }
            if (option == "gain-secrecy")
                hero.GainSecrecy(1, int.MaxValue);
            else if (option == "spend-grace")
                hero.SpendGrace(1);
            else if (option == "lose-secrecy")
                hero.LoseSecrecy(1, "Event");
            else if (option != "cont")
                throw new ArgumentOutOfRangeException(nameof(option), option, "Unknown option.");
            hero.AcceptRoll();
        }

        public class SloppySearchRollHandler : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                var e = hero.CurrentEvent;
                e.Rows.Activate(rollState.Result);
                if (rollState.Result == 6)
                    e.Options = new List<HeroEventOption> {new HeroEventOption("gain-secrecy", "Gain Secrecy")};
                else if (rollState.Result > 3)
                    e.Options = new List<HeroEventOption> { HeroEventOption.Continue() };
                else
                    e.Options = new List<HeroEventOption> { new HeroEventOption("spend-grace", "Spend Grace"), new HeroEventOption("lose-secrecy", "Lose Secrecy") };
                hero.PresentCurrentEvent();
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                hero.EndEvent();
            }
        }
    }
}

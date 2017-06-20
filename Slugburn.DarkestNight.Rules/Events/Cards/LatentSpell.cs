using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class LatentSpell : IEventCard
    {
        public string Name => "Latent Spell";
        public int Fate { get; }

        public EventDetail Detail => EventDetail.Create(x => x
            .Text("Lose 1 Secrecy. Then, spend 1 Grace or discard this event without further effect.",
                "Roll 1 die and take the highest",
                "6: Destroy a blight of your choice anywhere on the board",
                "5: Draw a power card",
                "4: Move to any other location",
                "1-3: No effect")
            .Option("grace", "Spend Grace", hero=>hero.Grace > 0)
            .Option("discard", "Discard Event"));

        public void Resolve(Hero hero, string option)
        {
            if (option == "grace")
            {
                hero.SpendGrace(1);
                hero.RollEventDice(new LatentSpellRollHandler());
            }
            else if (option == "discard")
            {
                hero.EndEvent();
            }
        }

        public class LatentSpellRollHandler : IRollHandler
        {
            public void HandleRoll(Hero hero)
            {
                var roll = hero.Roll;
                var result = roll.Max();
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
            }
        }
    }
}

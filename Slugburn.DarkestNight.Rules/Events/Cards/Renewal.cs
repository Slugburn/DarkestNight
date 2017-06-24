using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class Renewal : IEventCard
    {
        public EventDetail Detail => EventDetail.Create("Renewal", 0, x=>x.Text("Reshuffle the Event Deck and draw another card.").Option("cont", "Continue"));

        public void Resolve(Hero hero, string option)
        {
            var game = hero.Game;
            // shuffle until Renewal is not the top card
            do
            {
                game.Events = EventFactory.GetEventDeck().Shuffle();
            } while (game.Events.First() == "Renewal");
            hero.DrawEvent();
        }
    }
}

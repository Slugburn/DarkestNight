using System;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class Renewal : IEvent
    {
        public string Name => "Renewal";
        public EventDetail Detail => EventDetail.Create(x=>x.Text("Reshuffle the Event Deck and draw another card.").Option("cont", "Continue"));

        public void Resolve(Hero hero, string option)
        {
            var game = hero.Game;
            game.Events = EventFactory.GetEventDeck().Shuffle();
            hero.DrawEvent();
        }
    }
}

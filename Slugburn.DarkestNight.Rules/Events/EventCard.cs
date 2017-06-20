using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events
{
    public class EventCard : IEventCard
    {
        private readonly Action<Hero, string> _resolve;

        public EventCard(string name, string text, Action<Hero> resolve)
        {
            _resolve = (hero, option) => resolve(hero);
            Name = name;
            Detail = EventDetail.Create(x=>x.Text(text).Option("cont", "Continue"));
        }

        public EventCard(string name, Action< EventDetail.EventDetailCreation> create, Action<Hero,string> resolve)
        {
            _resolve = resolve;
            Name = name;
            Detail = EventDetail.Create(create);
        }

        public string Name { get; }
        public int Fate { get; }
        public EventDetail Detail { get; }
        public void Resolve(Hero hero, string option)
        {
            _resolve(hero, option);
        }
    }
}

using System;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events
{
    public class EventCard : IEventCard
    {
        private readonly Action<Hero, string> _resolve;

        public EventCard(string name, int fate, string text, Action<Hero> resolve)
        {
            _resolve = (hero, option) => resolve(hero);
            Detail = EventDetail.Create(name, fate, x=>x.Text(text).Option("cont", "Continue"));
        }

        public EventCard(string name, int fate, Action< EventDetail.EventDetailCreation> create, Action<Hero,string> resolve)
        {
            _resolve = resolve;
            Detail = EventDetail.Create(name,fate, create);
        }

        public EventDetail Detail { get; }
        public void Resolve(Hero hero, string option)
        {
            _resolve(hero, option);
            hero.EndEvent();
        }
    }
}

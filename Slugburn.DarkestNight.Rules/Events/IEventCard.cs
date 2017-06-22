using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events
{
    public interface IEventCard
    {
        EventDetail Detail { get; }
        void Resolve(Hero hero, string option);
    }
}
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events
{
    public interface IEventCard
    {
        string Name { get; }
        int Fate { get; }
        EventDetail Detail { get; }
        void Resolve(Hero hero, string option);
    }
}
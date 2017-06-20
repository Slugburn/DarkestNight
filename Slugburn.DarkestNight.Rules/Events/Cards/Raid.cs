using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class Raid : IEventCard
    {
        public string Name => "Raid";
        public int Fate { get; }

        public EventDetail Detail => EventDetail.Create(x => x
            .Text("Count the blights in your location",
                "0-1: Lose 2 Secrecy",
                "2-3: Lose 1 Grace and 1 Secrecy",
                "4: +1 Darkness")
            .Option("cont", "Continue"));

        public void Resolve(Hero hero, string option)
        {
            var count = hero.GetBlights().Count;
            if (count< 2)
            {
                hero.LoseSecrecy(2, "Event");
                return;
            }
            if (count < 4)
            {
                hero.LoseGrace();
                hero.LoseSecrecy("Event");
                return;
            }
            hero.Game.IncreaseDarkness();
        }
    }
}

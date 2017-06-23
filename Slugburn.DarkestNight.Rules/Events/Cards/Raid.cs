using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class Raid : IEventCard
    {
        public EventDetail Detail => EventDetail.Create("Raid", 6,
            x => x
                .Text("Count the blights in your location")
                .RowSelector(h => h.GetBlights().Count)
                .Row(0, 1, "Lose 2 Secrecy")
                .Row(2, 3, "Lose 1 Grace and 1 Secrecy")
                .Row(4, "+1 Darkness")
                .Option("cont", "Continue"));

        public void Resolve(Hero hero, string option)
        {
            var count = hero.GetBlights().Count;
            if (count< 2)
            {
                hero.LoseSecrecy(2, "Event");
            }
            else if (count < 4)
            {
                hero.LoseGrace();
                hero.LoseSecrecy("Event");
            }
            else
            {
                hero.Game.IncreaseDarkness();
            }
            hero.EndEvent();
        }
    }
}

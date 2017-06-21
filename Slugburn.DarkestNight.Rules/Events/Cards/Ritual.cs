using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class Ritual : IEventCard
    {
        public string Name => "Ritual";
        public int Fate { get; }

        public EventDetail Detail => EventDetail.Create(x => x.Text(
            "You may spend 1 Grace and lose 1 Secrecy to cancel this event.\nCount the blights in your location")
            .Row(0, "Necromancer moves there")
            .Row(1, 2, "New blight there")
            .Row(3, 4, "+1 Darkness")
            .Option("cancel", "Cancel Event", hero => hero.Grace > 0)
            .Option("cont", "Continue"));

        public void Resolve(Hero hero, string option)
        {
            if (option == "cancel")
            {
                hero.SpendGrace(1);
                hero.LoseSecrecy("Event");
                return;
            }
            var count = hero.GetBlights().Count;
            var game = hero.Game;
            if (count == 0)
                game.Necromancer.Location = hero.Location;
            else if (count < 3)
                game.CreateBlight(hero.Location);
            else
                game.IncreaseDarkness();
        }
    }
}

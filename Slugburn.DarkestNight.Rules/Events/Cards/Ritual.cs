using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class Ritual : IEventCard
    {
        public EventDetail Detail => EventDetail.Create("Ritual", 6, x => x
            .Text("You may spend 1 Grace and lose 1 Secrecy to cancel this event.\nCount the blights in your location")
            .RowSelector(hero => hero.GetBlights().Count)
            .Row(0, "Necromancer moves there")
            .Row(1, 2, "New blight there")
            .Row(3, 4, "+1 Darkness")
            .Option("cancel", "Cancel", hero => hero.CanSpendGrace)
            .Option("cont", "Continue"));

        public void Resolve(Hero hero, string option)
        {
            if (option == "cancel")
            {
                hero.SpendGrace(1);
                hero.LoseSecrecy(1);
                hero.EndEvent();
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
            hero.EndEvent();
        }
    }
}

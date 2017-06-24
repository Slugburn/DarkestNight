using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class UnfriendlyEyes : IEventCard
    {
        public EventDetail Detail => EventDetail.Create("Unfriendly Eyes", 5, x => x
            .Text("Count the blights in your location")
            .RowSelector(h => h.GetBlights().Count)
            .Row(0, "Lose 1 Secrecy", o => o.Option("lose-secrecy", "Lose Secrecy"))
            .Row(1, 2, "Spend 1 Secrecy or lose 1 Grace", o => o
                .Option("spend-secrecy", "Spend Secrecy", hero => hero.Secrecy > 0)
                .Option("lose-grace", "Lose Grace"))
            .Row(3, 4, "Spend 1 Grace or +1 Darkness", o => o
                .Option("spend-grace", "Spend Grace", hero => hero.Grace > 0)
                .Option("increase-darkness", "+1 Darkness")));

        public void Resolve(Hero hero, string option)
        {
            switch (option)
            {
                case "lose-secrecy":
                    hero.LoseSecrecy(1);
                    break;
                case "spend-secrecy":
                    hero.SpendSecrecy(1);
                    break;
                case "lose-grace":
                    hero.LoseGrace(1);
                    break;
                case "spend-grace":
                    hero.SpendGrace(1);
                    break;
                case "increase-darkness":
                    hero.Game.IncreaseDarkness();
                    break;
            }
            hero.EndEvent();
        }
    }
}
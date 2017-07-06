namespace Slugburn.DarkestNight.Rules.Events.Cards.Enemies
{
    public class BlackBanner : EnemyEventCard
    {
        public BlackBanner() : base(
            "Black Banner", 4,
            x => x.Text("Count the blights in your location")
                .RowSelector(hero => hero.GetBlights().Count)
                .Enemy(0, 1, "Archer")
                .Enemy(2, 3, "Lich")
                .Enemy(4, "Reaper"))
        {
        }
    }
}
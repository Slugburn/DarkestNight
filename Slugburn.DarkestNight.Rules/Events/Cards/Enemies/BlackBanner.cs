namespace Slugburn.DarkestNight.Rules.Events.Cards.Enemies
{
    public class BlackBanner : EnemyEventCard
    {
        public BlackBanner() : base(
            "Black Banner", 4,
            hero => hero.GetBlights().Count,
            x => x.Text("Count the blights in your location")
                .Enemy(0, 1, "Archer")
                .Enemy(2, 3, "Lich")
                .Enemy(4, "Reaper"))
        {
        }
    }
}

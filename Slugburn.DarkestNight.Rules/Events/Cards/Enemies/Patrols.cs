namespace Slugburn.DarkestNight.Rules.Events.Cards.Enemies
{
    public class Patrols : EnemyEventCard
    {
        public Patrols() : base(
            "Patrols", 4,
            hero => hero.Game.Darkness,
            x => x.Text("Compare to Darkness")
                .Enemy(0, 14, "Archer")
                .Enemy(15, 24, "Lich")
                .Enemy(25, 30, "Reaper"))
        {
        }
    }
}
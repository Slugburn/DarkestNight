namespace Slugburn.DarkestNight.Rules.Events.Cards.Enemies
{
    public class DarkChampion : EnemyEventCard
    {
        public DarkChampion() : base(
            "Dark Champion", 3,
            hero => hero.Game.Darkness,
            x => x.Text("Compare to Darkness")
                .Enemy(0, 9, "Ghoul")
                .Enemy(10, 19, "Revenant")
                .Enemy(20, 30, "Slayer"))
        {
        }
    }
}
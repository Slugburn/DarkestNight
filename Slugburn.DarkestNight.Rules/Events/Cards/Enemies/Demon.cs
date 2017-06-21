namespace Slugburn.DarkestNight.Rules.Events.Cards.Enemies
{
    public class Demon : EnemyEventCard
    {
        public Demon() : base(
            "Demon", 3,
            hero => hero.Secrecy, 
            x => x.Text("Compare to Secrecy")
                .Enemy(6, int.MaxValue, "Flying Demon")
                .Enemy(4, 5, "Fearful Demon")
                .Enemy(0, 3, "Deadly Demon"))
        {
        }
    }
}
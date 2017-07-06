namespace Slugburn.DarkestNight.Rules.Events.Cards.Enemies
{
    public class Horde : EnemyEventCard
    {
        public Horde() : base(
            "Horde", 3,
            x => x.Text("Compare to Secrecy")
                .RowSelector(hero => hero.Secrecy)
                .Enemy(4, int.MaxValue, "Small Horde")
                .Enemy(2, 3, "Large Horde")
                .Enemy(0, 1, "Giant Horde"))
        {
        }
    }
}
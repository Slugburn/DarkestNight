namespace Slugburn.DarkestNight.Rules.Events.Cards.Enemies
{
    public class Horde : EnemyEventCard
    {
        public Horde() : base(
            "Horde", 3,
            hero => hero.Secrecy,
            x => x.Text("Compare to Secrecy")
                .Enemy(4, int.MaxValue, "Small Horde")
                .Enemy(2, 3, "Large Horde")
                .Enemy(0, 1, "Giant Horde"))
        {
        }
    }
}
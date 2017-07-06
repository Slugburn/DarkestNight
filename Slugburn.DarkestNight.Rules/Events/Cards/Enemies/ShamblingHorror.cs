namespace Slugburn.DarkestNight.Rules.Events.Cards.Enemies
{
    public class ShamblingHorror : EnemyEventCard
    {
        public ShamblingHorror() : base(
            "Shambling Horror", 4,
            x => x.Text("Compare to Secrecy")
                .RowSelector(hero => hero.Secrecy)
                .Enemy(6, int.MaxValue, "Zombie")
                .Enemy(4, 5, "Mummy")
                .Enemy(0, 3, "Slayer"))
        {
        }
    }
}
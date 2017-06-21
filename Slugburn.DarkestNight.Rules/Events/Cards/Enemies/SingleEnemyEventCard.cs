namespace Slugburn.DarkestNight.Rules.Events.Cards.Enemies
{
    public class SingleEnemyEventCard : EnemyEventCard
    {
        public SingleEnemyEventCard(string name, int fate) : base(name, fate, x=>x.Enemy(name))
        {
        }
    }
}

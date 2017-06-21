namespace Slugburn.DarkestNight.Rules.Events.Cards.Enemies
{
    public class VengefulSpirit : EnemyEventCard
    {
        public VengefulSpirit() : base(
            "Vengeful Spirit", 4,
            hero => hero.Secrecy,
            x => x.Text("Compare to Secrecy")
                .Enemy(5, int.MaxValue, "Shade")
                .Enemy(3, 4, "Shadow")
                .Enemy(0, 2, "Hunter"))
        {
        }
    }
}
﻿namespace Slugburn.DarkestNight.Rules.Events.Cards.Enemies
{
    public class DeadServant : EnemyEventCard
    {
        public DeadServant() : base(
            "Dead Servant", 3,
            x => x.Text("Compare to Secrecy")
                .RowSelector(hero => hero.Secrecy)
                .Enemy(5, int.MaxValue, "Scout")
                .Enemy(3, 4, "Archer")
                .Enemy(0, 2, "Dread"))
        {
        }
    }
}
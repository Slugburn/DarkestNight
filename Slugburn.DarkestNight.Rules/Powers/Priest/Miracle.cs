using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Miracle : Bonus
    {
        public Miracle()
        {
            Name = "Miracle";
            Text = "Spend 1 Grace to reroll any die roll you make. You may do this repeatedly.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddAction(new MiracleAction(this));
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.Grace > 0 && hero.CurrentRoll?.ActualRoll != null;
        }

        internal class MiracleAction : PowerAction
        {
            public MiracleAction(IPower power) : base(power)
            {
            }

            public override void Act(Hero hero)
            {
                hero.SpendGrace(1);
                var roll = Die.Roll();
                hero.CurrentRoll.ActualRoll.Add(roll);
                hero.CurrentRoll.AdjustRoll();
            }
        }
    }
}
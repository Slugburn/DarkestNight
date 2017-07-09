using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Miracle : BonusPower
    {
        public Miracle()
        {
            Name = "Miracle";
            Text = "Spend 1 Grace to reroll any die roll you make. You may do this repeatedly.";
        }

        protected override void OnLearn()
        {
            Owner.AddAction(new MiracleAction(this));
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.Grace > 0 && hero.CurrentRoll?.ActualRoll != null;
        }

        internal class MiracleAction : PowerCommand
        {
            public MiracleAction(IPower power) : base(power, false)
            {
            }

            public override void Execute(Hero hero)
            {
                hero.SpendGrace(1);
                var roll = Die.Roll();
                hero.CurrentRoll.ActualRoll.Add(roll);
                hero.CurrentRoll.AdjustRoll();
            }
        }
    }
}
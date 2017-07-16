using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Rogue
{
    internal class Ambush : TacticPower
    {
        public Ambush()
        {
            Name = "Ambush";
            StartingPower = true;
            Text = "Spend 1 Secrecy to fight with 3 dice.";
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.CanSpendSecrecy;
        }

        protected override void OnLearn()
        {
            base.OnLearn();
            Owner.AddTactic(new AmbushTactic(this));
        }

        private class AmbushTactic : PowerTactic
        {
            public AmbushTactic(Ambush ambush) :base(ambush, TacticType.Fight, 3)
            {
            }

            public override void Use(Hero hero)
            {
                base.Use(hero);
                hero.SpendSecrecy(1);
            }
        }
    }
}
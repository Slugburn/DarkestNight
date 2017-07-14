using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Prince {
    class Strategy : TacticPower
    {
        public Strategy()
            : base()
        {
            Name = "Strategy";
            StartingPower = true;
            Text = "Fight with 2d.";
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero);
        }

        //            public override void Activate()
        //            {
        //                Hero.SetDice(RollType.Fight, 2);
        //            }
    }
}
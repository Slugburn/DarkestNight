using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Prince {
    class Scouts : ActionPower
    {
        public Scouts()
        {
            Name = "Scouts";
            Text = "Spend 1 Secrecy to activate in your location.";
            ActiveText = "Heroes gain +1d in searches there.";
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.Secrecy > 0;
        }

        //            public override void Activate()
        //            {
        //                var space = Hero.GetSpace();
        //                var bonus = new RollBonus(RollType.Search, BonusType.Dice, 1, this);
        //                Stash.Add(space, bonus);
        //                space.Add(bonus);
        //                Hero.LoseSecrecy();
        //            }
        //
        //            public override void Deactivate()
        //            {
        //                var space = Stash.Get<ISpace>();
        //                var bonus = Stash.Get<RollBonus>();
        //                space.Remove(bonus);
        //            }
    }
}
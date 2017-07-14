using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Prince {
    class Resistance : ActionPower
    {
        public Resistance()
        {
            Name = "Resistance";
            StartingPower = true;
            Text = "Spend 1 Secrecy to activate in your location.";
            ActiveText = "Heroes gain +1d in fights when attacking blights there.";
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.Secrecy > 0;
        }

        //            public override void Activate()
        //            {
        //                Hero.LoseSecrecy();
        //                var space = Hero.GetSpace();
        //                var bonus = new RollBonus(RollType.Fight, BonusType.Dice, 1, this) { Restriction = hero => hero.State == HeroState.AttackingBlight };
        //                space.Add(bonus);
        //                Stash.Add(space, bonus);
        //            }
        //
        //            public override void Deactivate()
        //            {
        //                base.Deactivate();
        //                var space = Stash.Get<ISpace>();
        //                var bonus = Stash.Get<RollBonus>();
        //                space.Remove(bonus);
        //            }
    }
}
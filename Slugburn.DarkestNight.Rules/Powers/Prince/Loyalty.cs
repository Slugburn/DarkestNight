namespace Slugburn.DarkestNight.Rules.Powers.Prince {
    class Loyalty : BonusPower
    {
        public Loyalty()
        {
            Name = "Loyalty";
            StartingPower = true;
            Text = "+1d when eluding.";
        }

        //            public override void Activate()
        //            {
        //                var bonus = new RollBonus(RollType.Elude, BonusType.Dice, 1, this);
        //                Hero.Add(bonus);
        //                Stash.Set(bonus);
        //            }
        //
        //            public override void Deactivate()
        //            {
        //                var bonus = Stash.Get<RollBonus>();
        //                Hero.Remove(bonus);
        //            }
    }
}
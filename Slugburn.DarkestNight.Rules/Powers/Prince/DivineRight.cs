namespace Slugburn.DarkestNight.Rules.Powers.Prince {
    class DivineRight : BonusPower
    {
        public DivineRight()
        {
            Name = "Divine Right";
            Text = "+1 to default Grace. Add 1 to each die when praying.";
        }

        //            public override void Activate()
        //            {
        //                Hero.DefaultGrace++;
        //                var bonus = new RollBonus(RollType.Pray, BonusType.Results, 1, this);
        //                Hero.Add(bonus);
        //                Stash.Set(bonus);
        //            }
        //
        //            public override void Deactivate()
        //            {
        //                base.Deactivate();
        //                Hero.DefaultGrace--;
        //                var bonus = Stash.Get<RollBonus>();
        //                Hero.Remove(bonus);
        //            }
    }
}
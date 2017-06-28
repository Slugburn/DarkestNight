namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Miracle : Bonus
    {
        public Miracle()
        {
            Name = "Miracle";
            Text = "Spend 1 Grace to reroll any die roll you make. You may do this repeatedly.";
        }

        //            public override void Activate()
        //            {
        //                var action = new TriggeredAction(Trigger.AfterRoll, Text,
        //                    hero => hero.OfferReroll().OnReroll(x => hero.LoseGrace())) { Condition = hero => hero.CanSpendGrace };
        //                Hero.Add(action);
        //                Stash.Set(action);
        //            }
        //
        //            public override void Deactivate()
        //            {
        //                base.Deactivate();
        //                var action = Stash.Get<TriggeredAction>();
        //                Hero.Remove(action);
        //            }
    }
}
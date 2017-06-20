namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Calm : Bonus
    {
        public Calm()
        {
            Name = "Calm";
            Text = "Heroes at your location may pray.";
        }

        //            public override void Activate()
        //            {
        //                var space = Hero.GetSpace();
        //                var action = new LocationAction(ActionType.Pray, this);
        //                Stash.Set(space, action);
        //                space.Add(action);
        //            }
        //
        //            public override void Deactivate()
        //            {
        //                base.Deactivate();
        //                var space = Stash.Get<ISpace>();
        //                var action = Stash.Get<LocationAction>();
        //                space.Remove(action);
        //            }
    }
}
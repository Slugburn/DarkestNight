using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Prince {
    class Chapel : ActionPower
    {
        public Chapel()
        {
            Name = "Chapel";
            Text = "Spend 1 Secrecy to activate in your location.";
            ActiveText = "Heroes may pray there.";
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.Secrecy > 0;
        }

//            public override void Activate()
//            {
//                var space = Hero.GetSpace();
//                var locationAction = new LocationAction(ActionType.Pray, this);
//                Stash.Add(space, locationAction);
//
//                space.Add(locationAction);
//                Hero.LoseSecrecy();
//            }
//
//            public override void Deactivate()
//            {
//                base.Deactivate();
//                var space = Stash.Get<ISpace>();
//                var locationAction = Stash.Get<LocationAction>();
//                space.Remove(locationAction);
//            }
    }
}
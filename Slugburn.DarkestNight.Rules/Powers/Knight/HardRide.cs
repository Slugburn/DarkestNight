using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class HardRide : ActionPower
    {
        public HardRide()
        {
            Name = "Hard Ride";
            StartingPower = true;
            Text = "Move twice, but gain no Secrecy.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddAction(new HardRideAction { Name = Name });
        }

        private class HardRideAction : PowerAction
        {
            public override void Act(Hero hero)
            {
                hero.State = HeroState.Moving;
                hero.AvailableMovement = 2;
                hero.IsActionAvailable = false;
            }
        }
    }
}
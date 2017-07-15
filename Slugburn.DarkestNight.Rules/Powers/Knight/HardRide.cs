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

        protected override void OnLearn()
        {
            Owner.AddCommand(new HardRideAction(this));
        }

        private class HardRideAction : PowerAction
        {
            public HardRideAction(IActionPower power) : base(power)
            {
            }

            public override async void Execute(Hero hero)
            {
                hero.AvailableMovement = 2;
                await TravelHandler.UseAvailableMovement(hero);
            }

        }
    }
}
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Spaces;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    class FalseOrders : ActionPower
    {
        private const string PowerName = "False Orders";

        public FalseOrders()
        {
            Name = PowerName;
            Text = "Move any number of blights from your location to one adjacent location, if this does not result in over 4 blights at one location.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddAction(new FalseOrdersAction());
        }

        private class FalseOrdersAction : PowerAction
        {
            public FalseOrdersAction() : base(PowerName)
            {
            }

            public override void Act(Hero hero)
            {
                var space = (Space)hero.GetSpace();
                var potentialDestinations = space.AdjacentLocations;
                var destination = hero.Player.ChooseLocation(potentialDestinations);
                if (destination == Location.None)
                    return;
                var destinationSpace = hero.Game.Board[destination];
                var maxMoveCount = 4 - hero.Game.Board[destination].Blights.Count();
                var blights = hero.Player.ChooseBlights(space.Blights, 1, maxMoveCount);
                if (!blights.Any())
                    return;
                foreach (var blight in blights)
                {
                    space.RemoveBlight(blight);
                    destinationSpace.AddBlight(blight);
                }
                hero.IsActionAvailable = false;
            }

        }
    }
}
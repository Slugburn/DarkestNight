using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    internal class FalseOrders : ActionPower
    {
        private const string PowerName = "False Orders";

        public FalseOrders()
        {
            Name = PowerName;
            Text = "Move any number of blights from your location to one adjacent location, if this does not result in over 4 blights at one location.";
        }

        protected override void OnLearn()
        {
            Owner.AddCommand(new FalseOrdersAction(this));
        }

        private class FalseOrdersAction : PowerAction
        {
            public FalseOrdersAction(IActionPower power) : base(power)
            {
            }

            public override async void Execute(Hero hero)
            {
                var space = hero.Space;
                var potentialDestinations = space.AdjacentLocations.Select(x => x.ToString()).ToList();
                var destination = await hero.SelectLocation(potentialDestinations);
                var game = hero.Game;

                var destinationSpace = game.Board[destination];
                var maxMoveCount = 4 - destinationSpace.Blights.Count;
                var selection = BlightSelectionModel.Create(Name, space.Blights, maxMoveCount);
                var blightIds = await hero.SelectBlights(selection);

                foreach (var blightId in blightIds)
                {
                    var blight = game.GetBlight(blightId);
                    var sourceSpace = game.Board[blight.Location];
                    sourceSpace.RemoveBlight(blight);

                    destinationSpace.AddBlight(blight);
                }
                game.UpdatePlayerBoard();
                hero.ContinueTurn();
            }
        }
    }
}
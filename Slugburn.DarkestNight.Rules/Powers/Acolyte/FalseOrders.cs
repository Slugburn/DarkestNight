using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;

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

        private class FalseOrdersAction : PowerAction, ICallbackHandler<IEnumerable<int>>
        {
            public FalseOrdersAction(IActionPower power) : base(power)
            {
            }

            private Location Destination { get; set; } // This gets set by the location selected handler

            public override async Task ExecuteAsync(Hero hero)
            {
                var space = hero.Space;
                var potentialDestinations = space.AdjacentLocations.Select(x => x.ToString()).ToList();
                var location = await hero.SelectLocation(potentialDestinations);
                var game = hero.Game;
                var action = (FalseOrdersAction)hero.GetCommand(PowerName);
                action.Destination = location;

                var destinationSpace = game.Board[location];
                var maxMoveCount = 4 - destinationSpace.Blights.Count;
                var callback = Callback.For<IEnumerable<int>>(hero, this);
                var selection = BlightSelectionModel.Create(Name, space.Blights, maxMoveCount, callback);
                hero.SelectBlights(selection);
            }

            public void HandleCallback(Hero hero, IEnumerable<int> blightIds)
            {
                var game = hero.Game;
                var action = (FalseOrdersAction) hero.GetCommand(PowerName);
                var destination = action.Destination;
                foreach (var blightId in blightIds)
                {
                    var blight = game.GetBlight(blightId);
                    var sourceSpace = game.Board[blight.Location];
                    sourceSpace.RemoveBlight(blight);

                    var destinationSpace = game.Board[destination];
                    destinationSpace.AddBlight(blight);
                }
                game.UpdatePlayerBoard();
                hero.ContinueTurn();
            }
        }
    }
}
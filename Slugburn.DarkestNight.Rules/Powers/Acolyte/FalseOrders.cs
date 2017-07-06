using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
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
            Owner.AddAction(new FalseOrdersAction(this));
        }

        private class FalseOrdersAction : PowerAction, ICallbackHandler
        {
            public FalseOrdersAction(IActionPower power) : base(power)
            {
            }

            private Location Destination { get; set; } // This gets set by the location selected handler

            public void HandleCallback(Hero hero, object data)
            {
                var game = hero.Game;
                if (data is Location)
                {
                    var action = (FalseOrdersAction) hero.GetCommand(PowerName);
                    var destination = (Location) data;
                    action.Destination = destination;

                    var destinationSpace = game.Board[destination];
                    var maxMoveCount = 4 - destinationSpace.Blights.Count;
                    var space = hero.Space;
                    var playerBlights = BlightModel.Create(space.Blights);
                    hero.Player.DisplayBlightSelection(new BlightSelectionModel(playerBlights, maxMoveCount), Callback.For(hero, this));
                }
                else if (data is IEnumerable<int>)
                {
                    var selection = (IEnumerable<int>) data;
                    var action = (FalseOrdersAction) hero.GetCommand(PowerName);
                    var destination = action.Destination;
                    foreach (var blightId in selection)
                    {
                        var blight = game.GetBlight(blightId);
                        var sourceSpace = game.Board[blight.Location];
                        sourceSpace.RemoveBlight(blight);

                        var destinationSpace = game.Board[destination];
                        destinationSpace.AddBlight(blight);
                    }
                }
            }

            public override void Execute(Hero hero)
            {
                var space = hero.Space;
                var potentialDestinations = space.AdjacentLocations.Select(x => x.ToString()).ToList();
                hero.Player.DisplayLocationSelection(potentialDestinations, Callback.For(hero, this));
            }
        }
    }
}
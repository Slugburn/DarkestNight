using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;

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

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddAction(new FalseOrdersAction(this));
        }

        private class FalseOrdersAction : PowerAction, ICallbackHandler
        {
            public FalseOrdersAction(IActionPower power) : base(power)
            {
            }

            private Location Destination { get; set; } // This gets set by the location selected handler

            public void HandleCallback(Hero hero, string path, object data)
            {
                if (data is Location)
                {
                    var action = (FalseOrdersAction) hero.GetCommand(PowerName);
                    var destination = (Location) data;
                    action.Destination = destination;

                    var destinationSpace = hero.Game.Board[destination];
                    var maxMoveCount = 4 - destinationSpace.Blights.Count;
                    var space = hero.GetSpace();
                    var playerBlights = space.Blights.Select(b => new PlayerBlight {Blight = b, Location = hero.Location}).ToList();
                    hero.Player.DisplayBlightSelection(new PlayerBlightSelection(playerBlights, maxMoveCount), Callback.ForCommand(hero, this));
                }
                else if (data is IEnumerable<BlightLocation>)
                {
                    var selection = (IEnumerable<BlightLocation>) data;
                    var action = (FalseOrdersAction) hero.GetCommand(PowerName);
                    var destination = action.Destination;
                    foreach (var blight in selection)
                    {
                        var sourceSpace = hero.Game.Board[blight.Location];
                        sourceSpace.RemoveBlight(blight.Blight);
                        var destinationSpace = hero.Game.Board[destination];
                        destinationSpace.AddBlight(blight.Blight);
                    }
                }
            }

            public override void Execute(Hero hero)
            {
                var space = hero.GetSpace();
                var potentialDestinations = space.AdjacentLocations.Select(x => x.ToString()).ToList();
                hero.Player.DisplayLocationSelection(potentialDestinations, Callback.ForCommand(hero, this));
            }
        }
    }
}
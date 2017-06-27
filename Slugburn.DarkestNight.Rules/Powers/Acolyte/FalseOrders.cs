using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;
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

        private class FalseOrdersAction : PowerAction, ICallbackHandler
        {
            public FalseOrdersAction() : base(PowerName)
            {
            }

            private Location Destination { get; set; }  // This gets set by the location selected handler

            public override void Act(Hero hero)
            {
                hero.IsActionAvailable = false;
                var space = (Space)hero.GetSpace();
                var potentialDestinations = space.AdjacentLocations.Select(x => x.ToString()).ToList();
                hero.Player.DisplayLocationSelection(potentialDestinations, Callback.ForAction(hero, this));
            }

            public void HandleCallback(Hero hero, string path, object data)
            {
                if (data is Location)
                {
                    var action = (FalseOrdersAction) hero.GetAction(PowerName);
                    var destination = (Location) data;
                    action.Destination = destination;

                    var destinationSpace = hero.Game.Board[destination];
                    var maxMoveCount = 4 - destinationSpace.Blights.Count;
                    var space = (Space) hero.GetSpace();
                    var playerBlights = space.Blights.Select(b => new PlayerBlight {Blight = b, Location = hero.Location}).ToList();
                    hero.Player.DisplayBlightSelection(new PlayerBlightSelection(playerBlights, maxMoveCount), Callback.ForAction(hero, this));
                }
                else if (data is IEnumerable<BlightLocation>)
                {
                    var selection = (IEnumerable<BlightLocation>) data;
                    var action = (FalseOrdersAction) hero.GetAction(PowerName);
                    var destination = action.Destination;
                    foreach (var blight in selection)
                    {
                        var sourceSpace = (Space) hero.Game.Board[blight.Location];
                        sourceSpace.RemoveBlight(blight.Blight);
                        var destinationSpace = (Space) hero.Game.Board[destination];
                        destinationSpace.AddBlight(blight.Blight);
                    }
                }

            }
        }
    }
}
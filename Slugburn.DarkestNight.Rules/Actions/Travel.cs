using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Travel : IAction, ICallbackHandler
    {
        private int _movesLeft;

        public string Name => "Travel";

        public string Text => @"Move to an adjacent location, and gain 1 Secrecy (up to 5).";

        public void Act(Hero hero)
        {
            _movesLeft = hero.TravelSpeed;
            UseTravelMovement(hero);
            hero.GainSecrecy(1, 5);
        }

        private void UseTravelMovement(Hero hero)
        {
            var validLocations = hero.GetValidMovementLocations().Select(x=>x.ToString()).ToList();
            hero.Player.DisplayLocationSelection(validLocations, Callback.ForAction(hero, this));
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsTakingTurn && hero.IsActionAvailable;
        }

        public void HandleCallback(Hero hero, string path, object data)
        {
            var location = (Location) data;
            hero.MoveTo(location);
            _movesLeft--;
            if (_movesLeft > 0)
                UseTravelMovement(hero);
        }
    }
}

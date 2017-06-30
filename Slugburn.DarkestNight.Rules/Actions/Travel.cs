using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Travel : StandardAction, ICallbackHandler
    {
        private int _movesLeft;

        public Travel() : base("Travel")
        {
            Text = "Move to an adjacent location, and gain 1 Secrecy (up to 5).";
        }

        public override void Execute(Hero hero)
        {
            _movesLeft = hero.TravelSpeed;
            UseTravelMovement(hero);
            hero.GainSecrecy(1, 5);
        }

        private void UseTravelMovement(Hero hero)
        {
            var validLocations = hero.GetValidMovementLocations().Select(x=>x.ToString()).ToList();
            hero.Player.DisplayLocationSelection(validLocations, Callback.ForCommand(hero, this));
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

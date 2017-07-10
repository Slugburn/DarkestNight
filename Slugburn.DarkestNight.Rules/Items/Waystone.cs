using System.Linq;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Items
{
    class Waystone : Item, ICommand, ICallbackHandler<Location>
    {
        public Waystone() : base("Waystone")
        {
            Text = "Discard during your turn to instantly move to any location and gain 1 Secrecy.";
        }

        public void Execute(Hero hero)
        {
            var destinations = hero.GetValidMovementLocations(false)
                .Except(new[] {hero.Location})
                .Select(loc => loc.ToString())
                .ToList();
            hero.SelectLocation(destinations, this);
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsTakingTurn;
        }

        public void HandleCallback(Hero hero, Location data)
        {
            var location = data;
            hero.MoveTo(location);
            hero.GainSecrecy(1, int.MaxValue);
            Owner.RemoveFromInventory(this);
            hero.ContinueTurn();
        }
    }
}

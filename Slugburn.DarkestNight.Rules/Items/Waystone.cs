using System.Linq;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Items
{
    class Waystone : Item, ICommand
    {
        public Waystone() : base("Waystone")
        {
            Text = "Discard during your turn to instantly move to any location and gain 1 Secrecy.";
        }

        public async Task ExecuteAsync(Hero hero)
        {
            var destinations = hero.GetValidMovementLocations(false)
                .Except(new[] {hero.Location})
                .Select(loc => loc.ToString())
                .ToList();
            var location  = await hero.SelectLocation(destinations);
            hero.MoveTo(location);
            hero.GainSecrecy(1, int.MaxValue);
            Owner.RemoveFromInventory(this);
            hero.ContinueTurn();
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsTakingTurn;
        }
    }
}

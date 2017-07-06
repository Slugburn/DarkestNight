using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Actions
{
    internal class TravelHandler : ICallbackHandler
    {
        public static void UseAvailableMovement(Hero hero)
        {
            var validLocations = hero.GetValidMovementLocations().Select(x => x.ToString()).ToList();
            hero.Player.DisplayLocationSelection(validLocations, Callback.For(hero, new TravelHandler()));
        }

        public void HandleCallback(Hero hero, object data)
        {
            var location = (Location)data;
            hero.MoveTo(location);
            hero.AvailableMovement--;
            if (hero.AvailableMovement > 0)
                UseAvailableMovement(hero);
        }

    }
}
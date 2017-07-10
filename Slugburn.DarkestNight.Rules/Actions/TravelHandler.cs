using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Actions
{
    internal class TravelHandler : ICallbackHandler<Location>
    {
        public static void UseAvailableMovement(Hero hero)
        {
            hero.State = HeroState.Moving;
            var validLocations = hero.GetValidMovementLocations().Select(x => x.ToString()).ToList();
            hero.SelectLocation(validLocations, new TravelHandler());
        }

        public void HandleCallback(Hero hero, Location data)
        {
            var location = data;
            hero.MoveTo(location);
            hero.AvailableMovement--;
            if (hero.AvailableMovement > 0)
            {
                UseAvailableMovement(hero);
            }
            else
            {
                hero.State = HeroState.FinishedMoving;
                hero.ContinueTurn();
            }
        }

    }
}
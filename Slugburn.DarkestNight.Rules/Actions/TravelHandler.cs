using System.Linq;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Actions
{
    internal class TravelHandler
    {
        public static async Task UseAvailableMovement(Hero hero)
        {
            while (hero.AvailableMovement > 0)
            {
                hero.State = HeroState.Moving;
                var validLocations = hero.GetValidMovementLocations().Select(x => x.ToString()).ToList();
                var location = await hero.SelectLocation(validLocations);
                hero.MoveTo(location);
                hero.AvailableMovement--;
            }
            hero.State = HeroState.FinishedMoving;
            hero.ContinueTurn();
        }
    }
}
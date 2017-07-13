using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Travel : StandardAction
    {
        public Travel() : base("Travel")
        {
            Text = "Move to an adjacent location, and gain 1 Secrecy (up to 5).";
        }

        public override Task ExecuteAsync(Hero hero)
        {
            hero.GainSecrecy(1, 5);
            hero.AvailableMovement = hero.TravelSpeed;
            return TravelHandler.UseAvailableMovement(hero);
        }
    }
}

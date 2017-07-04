using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Travel : StandardAction
    {
        public Travel() : base("Travel")
        {
            Text = "Move to an adjacent location, and gain 1 Secrecy (up to 5).";
        }

        public override void Execute(Hero hero)
        {
            hero.GainSecrecy(1, 5);
            hero.AvailableMovement = hero.TravelSpeed;
            TravelHandler.UseAvailableMovement(hero);
            hero.ContinueTurn();
        }
    }
}

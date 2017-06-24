using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class EndTurn : IAction
    {
        public string Name => "End Turn";
        public void Act(Hero hero)
        {
            hero.EndTurn();
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.Game.ActingHero == hero;
        }
    }
}

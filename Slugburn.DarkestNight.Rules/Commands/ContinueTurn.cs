using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Commands
{
    public class ContinueTurn : IStartOfTurnCommand
    {
        public string Name => "Continue";
        public string Text => "Continue turn";

        public bool IsAvailable(Hero hero)
        {
            return hero.IsTakingTurn && hero.State == HeroState.TurnStarted;
        }

        public void Execute(Hero hero)
        {
            hero.ContinueTurn();
        }
    }
}

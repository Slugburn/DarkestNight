using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Commands
{
    public class ContinueTurn : IStartOfTurnCommand
    {
        public string Name => "Continue";
        public string Text => "Continue turn";
        public bool RequiresAction => false;

        public bool IsAvailable(Hero hero)
        {
            return hero.IsTakingTurn && hero.State == HeroState.TurnStarted;
        }

        public Task ExecuteAsync(Hero hero)
        {
            hero.ContinueTurn();
            return Task.CompletedTask;
        }
    }
}

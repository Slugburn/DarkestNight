using System.Linq;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Commands
{
    public class StartTurn : ICommand
   {
        public string Name => "Start Turn";

        public string Text => null;
        public bool RequiresAction => false;

        public Task ExecuteAsync(Hero hero)
        {
            if (IsAvailable(hero))
                hero.StartTurn();
            return Task.CompletedTask;
        }

        public bool IsAvailable(Hero hero)
        {
            var game = hero.Game;
            return !hero.HasTakenTurn
                   && !game.Necromancer.IsTakingTurn
                   && !game.Heroes.Any(h => h.IsTakingTurn);
        }
   }
}

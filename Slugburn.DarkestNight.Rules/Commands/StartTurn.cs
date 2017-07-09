using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Commands
{
    public class StartTurn : ICommand
   {
        public string Name => "Start Turn";

        public string Text => null;
        public bool RequiresAction => false;

        public void Execute(Hero hero)
        {
            if (!IsAvailable(hero)) return;
            hero.StartTurn();
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

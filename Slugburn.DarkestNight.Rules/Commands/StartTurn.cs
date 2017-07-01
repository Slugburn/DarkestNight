using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Commands
{
    public class StartTurn : ICommand
   {
        public string Name => "Start Turn";

        public string Text => null;

        public void Execute(Hero hero)
        {
            if (!IsAvailable(hero)) return;
            hero.StartTurn();
        }

        public bool IsAvailable(Hero hero)
        {
            return IsValid(hero);
        }

        internal static bool IsValid(Hero hero)
        {
            var game = hero.Game;
            return !hero.HasTakenTurn
                   && !game.Necromancer.IsTakingTurn
                   && !game.Heroes.Any(h => h.IsTakingTurn);
        }
   }
}

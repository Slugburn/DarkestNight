using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Items;
using Slugburn.DarkestNight.Rules.Triggers;

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
            var game = hero.Game;
            return !hero.HasTakenTurn
                   && !game.Necromancer.IsTakingTurn
                   && !game.Heroes.Any(h => h.IsTakingTurn);
        }
    }
}

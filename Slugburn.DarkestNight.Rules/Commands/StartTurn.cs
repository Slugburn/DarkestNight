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
            hero.Game.ActingHero = hero;
            hero.Triggers.Send(HeroTrigger.StartedTurn);
            var necromancer = hero.Game.Necromancer;
            var withNecromancer = hero.Location == necromancer.Location;
            if (withNecromancer)
                hero.LoseSecrecy("Necromancer");
            if (hero.GetInventory().Any(item => item is HolyRelic))
                hero.LoseSecrecy("Holy Relic");
            hero.UpdateAvailableActions();

            if (withNecromancer && hero.Secrecy == 0)
                hero.FaceEnemy(necromancer);
            else if (hero.Location != Location.Monastery)
                hero.DrawEvent();
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

using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    abstract class Blessing:ActivateablePower, ICallbackHandler
    {
        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            var validHeroes = hero.Game.Heroes.Where(h => h.Location == hero.Location);
            var view = new PlayerHeroSelection(validHeroes);
            hero.Player.DisplayHeroSelection(view, Callback.ForPower(hero, this));
        }

        public abstract void HandleCallback(Hero hero, string path, object data);
    }
}

using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    abstract class Blessing:ActivateablePower, ICallbackHandler<Hero>
    {
        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            var validHeroes = hero.Game.Heroes.Where(h => h.Location == hero.Location);
            var view = new HeroSelectionModel(validHeroes);
            hero.Player.DisplayHeroSelection(view, Callback.For(hero, this));
        }

        public abstract void HandleCallback(Hero hero, Hero data);
    }
}

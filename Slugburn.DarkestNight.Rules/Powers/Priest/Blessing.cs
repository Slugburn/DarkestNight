using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.IO;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    abstract class Blessing:ActivateablePower, ICallbackHandler<Hero>, IRestorable
    {
        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            var validHeroes = hero.Game.Heroes.Where(h => h.Location == hero.Location);
            var view = new HeroSelectionModel(validHeroes);
            hero.Player.DisplayHeroSelection(view, Callback.For(hero, this));
        }

        public void HandleCallback(Hero hero, Hero data)
        {
            Target = data;
            ActivateOnTarget();
        }

        public Hero Target { get; set; }

        protected abstract void ActivateOnTarget();

        public void Save(PowerData data)
        {
            data.IsActive = IsActive;
            data.Target = Target?.Name;
        }

        public void Restore(PowerData data)
        {
            IsActive = data.IsActive ?? false;
            if (!IsActive) return;

            if (data.Target == null) return;
            Target = Owner.Game.GetHero(data.Target);
            ActivateOnTarget();
        }
    }
}

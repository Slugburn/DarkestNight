using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    abstract class Blessing:ActivateablePower, ITargetable
    {
        public override async void Activate(Hero hero)
        {
            base.Activate(hero);
            if (Target == null)
            {
                var validHeroes = hero.Game.Heroes.Where(h => h.Location == hero.Location);
                Target = await hero.SelectHero(validHeroes);
            }
            ActivateOnTarget();
        }

        protected Hero Target { get; set; }

        protected abstract void ActivateOnTarget();

        public void SetTarget(string targetName)
        {
            Target = Owner.Game.GetHero(targetName);
        }

        public string GetTarget()
        {
            return Target?.Name;
        }
    }
}

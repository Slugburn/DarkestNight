using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players.Models;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class BlessingOfFaith : ActivateablePower
    {
        public BlessingOfFaith()
        {
            Name = "Blessing of Faith";
            StartingPower = true;
            Text = "Activate on a hero in your location.";
            ActiveText = "Gain an extra Grace (up to default) when praying.";
        }

        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            var validHeroes = hero.Game.Heroes.Where(h => h.Location == hero.Location);
            hero.SetHeroSelectionHandler(new BlessingOfFaithHeroSelection());

            var view = new PlayerHeroSelection(validHeroes);
            hero.Player.DisplayHeroSelection(view);
        }

        public override bool Deactivate(Hero hero)
        {
            return base.Deactivate(hero);
            throw new NotImplementedException();
        }

        internal class BlessingOfFaithHeroSelection : IHeroSelectionHandler
        {
            public void Handle(Hero hero, Hero selectedHero)
            {
                throw new NotImplementedException();
            }
        }
    }
}
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class BlessingOfFaith : ActivateablePower, ICallbackHandler
    {
        private const string PowerName = "Blessing of Faith";

        public BlessingOfFaith()
        {
            Name = PowerName;
            StartingPower = true;
            Text = "Activate on a hero in your location.";
            ActiveText = "Gain an extra Grace (up to default) when praying.";
        }

        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            var validHeroes = hero.Game.Heroes.Where(h => h.Location == hero.Location);
            var view = new PlayerHeroSelection(validHeroes);
            hero.Player.DisplayHeroSelection(view, Callback.ForPower(hero, this));
        }

        public void HandleCallback(Hero hero, string path, object data)
        {
            var selectedHero = (Hero) data;
            selectedHero.Triggers.Add(HeroTrigger.Praying, Name, new BlessingOfFaithWhenPraying(hero.Name) );
        }

        internal class BlessingOfFaithWhenPraying : ITriggerHandler<Hero>
        {
            private readonly string _ownerName;

            public BlessingOfFaithWhenPraying(string ownerName)
            {
                _ownerName = ownerName;
            }

            public void HandleTrigger(Hero hero, string source, TriggerContext context)
            {
                if (!hero.Game.GetHero(_ownerName).GetPower(PowerName).Exhausted)
                    hero.GainGrace(1, hero.DefaultGrace);
            }
        }
    }

}
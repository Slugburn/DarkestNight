using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class BlessingOfFaith : Blessing
    {
        private const string PowerName = "Blessing of Faith";

        public BlessingOfFaith()
        {
            Name = PowerName;
            StartingPower = true;
            Text = "Activate on a hero in your location.";
            ActiveText = "Gain an extra Grace (up to default) when praying.";
        }

        protected override void ActivateOnTarget()
        {
            Target.Triggers.Add(HeroTrigger.Prayed, Name, new BlessingOfFaithWhenPraying(Owner.Name));
        }

        internal class BlessingOfFaithWhenPraying : ITriggerHandler<Hero>
        {
            private readonly string _ownerName;

            public BlessingOfFaithWhenPraying(string ownerName)
            {
                _ownerName = ownerName;
            }

            public Task HandleTriggerAsync(Hero hero, string source, TriggerContext context)
            {
                if (!hero.Game.GetHero(_ownerName).GetPower(PowerName).IsExhausted)
                    hero.GainGrace(1, hero.DefaultGrace);
                return Task.CompletedTask;
            }
        }
    }

}
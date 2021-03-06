﻿using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class BlessingOfPiety : Blessing
    {
        public BlessingOfPiety()
        {
            Name = "Blessing of Piety";
            Text = "Activate on a hero in your location.";
            ActiveText = "Gain 1 Grace (up to default) when hiding.";
        }

        protected override void ActivateOnTarget()
        {
            Target.Triggers.Add(HeroTrigger.Hidden, Name, new BlessingOfPietyWhenHiding(this));
        }

        private class BlessingOfPietyWhenHiding : ITriggerHandler<Hero>
        {
            private readonly BlessingOfPiety _power;

            public BlessingOfPietyWhenHiding(BlessingOfPiety power)
            {
                _power = power;
            }

            public Task HandleTriggerAsync(Hero hero, string source, TriggerContext context)
            {
                if (!_power.IsExhausted)
                    hero.GainGrace(1, hero.DefaultGrace);
                return Task.CompletedTask;
            }
        }
    }

}
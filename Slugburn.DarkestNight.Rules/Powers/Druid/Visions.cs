using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class Visions : Bonus
    {
        private const string PowerName = "Visions";

        public Visions()
        {
            Name = PowerName;
            StartingPower = true;
            Text = "Exhaust after you draw an event card to discard it without effect.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.Triggers.Register(HeroTrigger.EventDrawn, new VisionsEventDrawnHandler());
            hero.Triggers.Register(HeroTrigger.EventOptionSelected, new VisionsEventOptionsSelectedHandler());
        }


        private class VisionsEventDrawnHandler : ITriggerHandler<Hero>
        {
            public string Name => PowerName;

            public void HandleTrigger(Hero registrar, TriggerContext context)
            {
                var hero = registrar;
                var power = hero.GetPower(PowerName);
                if (!power.IsUsable(hero)) return;
                var card = hero.CurrentEvent;
                if (!card.IsIgnorable) return;
                card.AddOption(PowerName, $"Ignore [{PowerName}]");
            }
        }

        private class VisionsEventOptionsSelectedHandler : ITriggerHandler<Hero>
        {
            public string Name => PowerName;

            public void HandleTrigger(Hero registrar, TriggerContext context)
            {
                var hero = registrar;
                var selectedOption = hero.CurrentEvent.SelectedOption;
                if (selectedOption != PowerName) return;

                var power = hero.GetPower(PowerName);
                if (!power.IsUsable(hero)) return;
                var card = hero.CurrentEvent;
                if (!card.IsIgnorable) return;

                context.Cancel = true;
                hero.CancelCurrentEvent();
                power.Exhaust(hero);
            }
        }
    }
}
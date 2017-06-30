using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class Visions : BonusPower
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
            hero.Triggers.Add(HeroTrigger.EventDrawn, Name, new VisionsEventDrawnHandler());
            hero.Triggers.Add(HeroTrigger.EventOptionSelected, Name, new VisionsEventOptionsSelectedHandler());
        }


        private class VisionsEventDrawnHandler : ITriggerHandler<Hero>
        {
            public void HandleTrigger(Hero registrar, string source, TriggerContext context)
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
            public void HandleTrigger(Hero registrar, string source, TriggerContext context)
            {
                var hero = registrar;
                var selectedOption = hero.CurrentEvent.SelectedOption;
                if (selectedOption != PowerName) return;

                var power = hero.GetPower(PowerName);
                if (!power.IsUsable(hero)) return;
                var card = hero.CurrentEvent;
                if (!card.IsIgnorable) return;

                context.Cancel = true;
                hero.EndEvent();
                power.Exhaust(hero);
            }
        }
    }
}
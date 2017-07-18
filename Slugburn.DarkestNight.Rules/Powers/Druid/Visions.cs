using System.Threading.Tasks;
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

        protected override void OnLearn()
        {
            Owner.Game.Triggers.Add(GameTrigger.EventDrawn, Name, new VisionsEventDrawnHandler(this));
            Owner.Game.Triggers.Add(GameTrigger.EventOptionSelected, Name, new VisionsEventOptionsSelectedHandler(this));
        }


        private class VisionsEventDrawnHandler : ITriggerHandler<Game>
        {
            private readonly Visions _visions;

            public VisionsEventDrawnHandler(Visions visions)
            {
                _visions = visions;
            }

            public Task HandleTriggerAsync(Game game, string source, TriggerContext context)
            {
                var hero = _visions.Owner;
                if (hero != game.ActingHero) return Task.CompletedTask;
                if (!_visions.IsUsable(hero)) return Task.CompletedTask;
                var card = hero.CurrentEvent;
                if (!card.IsIgnorable) return Task.CompletedTask;
                card.AddOption(PowerName, $"Ignore [{PowerName}]");
                return Task.CompletedTask;
            }
        }

        private class VisionsEventOptionsSelectedHandler : ITriggerHandler<Game>
        {
            private readonly Visions _visions;

            public VisionsEventOptionsSelectedHandler(Visions visions)
            {
                _visions = visions;
            }

            public Task HandleTriggerAsync(Game game, string source, TriggerContext context)
            {
                var hero = _visions.Owner;
                if (hero.CurrentEvent == null) return Task.CompletedTask;
                var selectedOption = hero.CurrentEvent.SelectedOption;
                if (selectedOption != PowerName) return Task.CompletedTask;

                if (!_visions.IsUsable(hero)) return Task.CompletedTask;
                var card = hero.CurrentEvent;
                if (!card.IsIgnorable) return Task.CompletedTask;

                context.Cancel = true;
                hero.EndEvent();
                _visions.Exhaust(hero);
                return Task.CompletedTask;
            }
        }
    }
}
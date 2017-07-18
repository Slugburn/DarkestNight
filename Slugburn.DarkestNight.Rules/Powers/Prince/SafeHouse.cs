using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Prince
{
    internal class SafeHouse : ActivateOnSpacePower
    {
        public SafeHouse()
        {
            Name = "Safe House";
            Text = "Spend 2 Secrecy to activate in your location.";
            ActiveText = "Heroes gain 1 Secrecy (up to 5) when ending a turn there, and +1d when eluding there.";
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.CanSpendSecrecy && hero.Secrecy > 1;
        }

        protected override void PayActivationCost()
        {
            Owner.SpendSecrecy(2);
        }

        protected override void ActivateEffect()
        {
            Target.AddModifier(new PowerRollBonus(this, ModifierType.EludeDice, 1));
            Owner.Game.Triggers.Add(GameTrigger.HeroTurnEnded, Name, new SafeHouseTurnEndedHandler(this));
        }

        internal class SafeHouseTurnEndedHandler : ITriggerHandler<Game>
        {
            private readonly SafeHouse _power;

            public SafeHouseTurnEndedHandler(SafeHouse power)
            {
                _power = power;
            }

            public Task HandleTriggerAsync(Game game, string source, TriggerContext context)
            {
                var hero = context.GetState<Hero>();
                if (!_power.IsExhausted && hero.Location == _power.Target.Location)
                    hero.GainSecrecy(1, 5);
                return Task.CompletedTask;
            }
        }
    }
}
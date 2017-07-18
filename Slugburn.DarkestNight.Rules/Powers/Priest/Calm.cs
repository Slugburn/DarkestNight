using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Calm : BonusPower, ITriggerHandler<Hero>
    {
        private CalmPray _action;

        public Calm()
        {
            Name = "Calm";
            Text = "Heroes at your location may pray.";
        }

        protected override void OnLearn()
        {
            _action = new CalmPray(this);
            var space = Owner.Space;
            space.AddAction(_action);
            Owner.Triggers.Add(HeroTrigger.Moving, Name, this );
            Owner.Triggers.Add(HeroTrigger.Moved, Name, this);
        }

        public Task HandleTriggerAsync(Hero hero, string source, TriggerContext context)
        {
            if (context.WasTriggeredBy(HeroTrigger.Moving))
                hero.Space.RemoveAction(_action.Name);
            else if (context.WasTriggeredBy(HeroTrigger.Moved))
                hero.Space.AddAction(_action);
            return Task.CompletedTask;
        }

        internal class CalmPray : StandardAction
        {
            private readonly Calm _power;
            private readonly Pray _pray;

            public CalmPray(Calm power) :base ("Pray [Calm]")
            {
                _power = power;
                _pray = new Pray();
                Text = _pray.Text;
            }

            public override bool IsAvailable(Hero hero)
            {
                return base.IsAvailable(hero)
                       && hero.Grace < hero.DefaultGrace
                       && hero.CanGainGrace()
                       && !_power.IsExhausted;
            }

            public override void Execute(Hero hero)
            {
                _pray.Execute(hero);
            }
        }

    }
}
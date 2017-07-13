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
            _action = new CalmPray();
            var space = Owner.Space;
            space.AddAction(_action);
            Owner.Triggers.Add(HeroTrigger.Moving, Name, this );
            Owner.Triggers.Add(HeroTrigger.Moved, Name, this);
        }

        public void HandleTrigger(Hero hero, string source, TriggerContext context)
        {
            if (context.WasTriggeredBy(HeroTrigger.Moving))
                hero.Space.RemoveAction(_action.Name);
            else if (context.WasTriggeredBy(HeroTrigger.Moved))
                hero.Space.AddAction(_action);
        }

        internal class CalmPray : StandardAction
        {
            private readonly Pray _pray;

            public CalmPray() :base ("Pray [Calm]")
            {
                _pray = new Pray();
                Text = _pray.Text;
            }

            public override Task ExecuteAsync(Hero hero)
            {
                return _pray.ExecuteAsync(hero);
            }
        }

    }
}
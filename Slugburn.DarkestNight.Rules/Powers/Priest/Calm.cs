using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Calm : Bonus, ITriggerHandler<Hero>
    {
        private CalmPray _action;

        public Calm()
        {
            Name = "Calm";
            Text = "Heroes at your location may pray.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            _action = new CalmPray();
            var space = hero.GetSpace();
            space.AddAction(_action);
            hero.Triggers.Add(HeroTrigger.Moving, Name, this );
            hero.Triggers.Add(HeroTrigger.Moved, Name, this);
        }

        public void HandleTrigger(Hero hero, string source, TriggerContext context)
        {
            if (context.WasTriggeredBy(HeroTrigger.Moving))
                hero.GetSpace().RemoveAction(_action);
            else if (context.WasTriggeredBy(HeroTrigger.Moved))
                hero.GetSpace().AddAction(_action);
        }

        internal class CalmPray : Pray
        {
            public CalmPray()
            {
                Name = "Pray [Calm]";
            }

            public override bool IsAvailable(Hero hero)
            {
                return hero.IsActing && hero.IsActionAvailable;
            }
        }

    }
}
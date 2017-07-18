using System.Collections.Generic;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class OathOfVengeance : Oath, IRollModifier
    {
        public OathOfVengeance()
        {
            Name = "Oath of Vengeance";
            ActiveText = "Add 1 to highest die when fighting the Necromancer.";
            FulfillText = "Win fight versus the Necromancer; you get a free action.";
            BreakText = "Hide or search; you lose 1 Grace.";
        }

        protected override void OnLearn()
        {
            base.OnLearn();
            Owner.AddRollModifier(this);
        }

        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            hero.Triggers.Add(HeroTrigger.FightWon, Name, new OathFulfilled());
            hero.Triggers.Add(HeroTrigger.Hidden, Name, new OathBroken());
            hero.Triggers.Add(HeroTrigger.Searched, Name, new OathBroken());
        }

        public override void Fulfill(Hero hero)
        {
            hero.AddFreeAction();
            Deactivate(hero);
        }

        public override void Break(Hero hero)
        {
            hero.LoseGrace();
            Deactivate(hero);
        }

        private class OathFulfilled : ITriggerHandler<Hero>
        {
            public Task HandleTriggerAsync(Hero hero, string source, TriggerContext context)
            {
                if (!hero.IsTargetNecromancer()) return Task.CompletedTask;
                var oath = (IOath) hero.GetPower(source);
                oath.Fulfill(hero);
                return Task.CompletedTask;
            }
        }

        private class OathBroken : ITriggerHandler<Hero>
        {
            public Task HandleTriggerAsync(Hero hero, string source, TriggerContext context)
            {
                var oath = (IOath)hero.GetPower(source);
                oath.Break(hero);
                return Task.CompletedTask;
            }
        }

        public ICollection<int> Modify(Hero hero, ModifierType modifierType, ICollection<int> roll)
        {
            if (modifierType != ModifierType.FightDice || IsExhausted || !IsActive) return roll;
            return hero.IsTargetNecromancer() ? roll.AddOneToHighest() : roll;
        }
    }

}
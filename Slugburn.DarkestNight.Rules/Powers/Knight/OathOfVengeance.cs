using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class OathOfVengeance : Oath
    {
        public OathOfVengeance()
        {
            Name = "Oath of Vengeance";
            ActiveText = "Add 1 to highest die when fighting the Necormancer.";
            FulfillText = "Win fight versus the Necromancer; you get a free action.";
            BreakText = "Hide or search; you lose 1 Grace.";
        }

        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            hero.Triggers.Add(HeroTrigger.AfterRoll, Name, new AfterRoll());
            hero.Triggers.Add(HeroTrigger.FightWon, Name, new OathFulfilled());
            hero.Triggers.Add(HeroTrigger.Hiding, Name, new OathBroken());
            hero.Triggers.Add(HeroTrigger.Searching, Name, new OathBroken());
        }

        public override void Fulfill(Hero hero)
        {
            hero.FreeActions++;
            Deactivate(hero);
        }

        public override void Break(Hero hero)
        {
            hero.LoseGrace();
            Deactivate(hero);
        }

        private class AfterRoll : ITriggerHandler<Hero>
        {
            public void HandleTrigger(Hero hero, string source, TriggerContext context)
            {
                if (hero.ConflictState == null) return;
                if (!hero.IsTargetNecromancer()) return;
                var originalRoll = hero.Roll.OrderByDescending(x=>x).ToList();
                var newRoll = new[] {originalRoll.First() + 1}.Concat(originalRoll.Skip(1)).ToList();
                hero.Roll = newRoll;
            }
        }

        private class OathFulfilled : ITriggerHandler<Hero>
        {
            public void HandleTrigger(Hero hero, string source, TriggerContext context)
            {
                if (!hero.IsTargetNecromancer()) return;
                var oath = (IOath) hero.GetPower(source);
                oath.Fulfill(hero);
            }
        }

        private class OathBroken : ITriggerHandler<Hero>
        {
            public void HandleTrigger(Hero hero, string source, TriggerContext context)
            {
                var oath = (IOath)hero.GetPower(source);
                oath.Break(hero);
            }
        }
    }

}
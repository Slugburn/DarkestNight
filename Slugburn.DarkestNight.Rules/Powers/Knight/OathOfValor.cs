using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class OathOfValor : Oath
    {
        public OathOfValor()
        {
            Name = "Oath of Valor";
            ActiveText = "+1 die in fights.";
            FulfillText = "Win a fight; You may activate any Oath immediately.";
            BreakText = "Attempt to elude; you lose 1 Grace.";
        }

        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            hero.AddModifier(StaticRollBonus.Create(Name, ModifierType.FightDice, 1));
            hero.Triggers.Add(HeroTrigger.FightWon, Name, new OathOfValorFulfilled() );
            hero.Triggers.Add(HeroTrigger.Eluding, Name, new OathOfValorBroken());
        }

        public override bool Deactivate(Hero hero)
        {
            if (!base.Deactivate(hero)) return false;
            hero.RemoveRollModifiers(Name);
            return true;
        }

        public override void Fulfill(Hero hero)
        {
            Deactivate(hero);
            hero.AddFreeAction(p=>p is IOath);
        }

        public override void Break(Hero hero)
        {
            hero.LoseGrace();
            Deactivate(hero);
        }

        private class OathOfValorFulfilled : ITriggerHandler<Hero>
        {
            public void HandleTrigger(Hero hero, string source, TriggerContext context)
            {
                var oath = (IOath)hero.GetPower(source);
                oath.Fulfill(hero);
            }
        }

        private class OathOfValorBroken : ITriggerHandler<Hero>
        {
            public void HandleTrigger(Hero hero, string source, TriggerContext context)
            {
                var oath = (IOath)hero.GetPower(source);
                oath.Break(hero);
            }
        }
    }
}
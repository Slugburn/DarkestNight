using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
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
            hero.AddRollModifier(new StaticRollBonus {Name = Name, RollType = RollType.Fight, DieCount = 1});
            hero.Triggers.Register(HeroTrigger.FightWon, new OathOfValorFulfilled {Name = Name} );
            hero.Triggers.Register(HeroTrigger.Eluding, new OathOfValorBroken {Name = Name});
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
            var oaths = hero.Powers.Where(x => x is IOath).Cast<IOath>();
            hero.State = HeroState.ChoosingAction;
            var availableOaths = oaths.Where(x=>x.IsUsable(hero)).Select(x => x.Name).ToList();
            if (availableOaths.Any())
            {
                hero.AvailableActions = availableOaths;
            }
        }

        public override void Break(Hero hero)
        {
            hero.LoseGrace();
            Deactivate(hero);
        }

        private class OathOfValorFulfilled : HeroTriggerHandler
        {
            public override void HandleTrigger(Hero hero, TriggerContext context)
            {
                var oath = (IOath)hero.GetPower(Name);
                oath.Fulfill(hero);
            }
        }

        private class OathOfValorBroken : HeroTriggerHandler
        {
            public override void HandleTrigger(Hero hero, TriggerContext context)
            {
                var oath = (IOath)hero.GetPower(Name);
                oath.Break(hero);
            }
        }
    }
}
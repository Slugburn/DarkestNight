using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class OathOfPurging : Oath
    {
        private const string PowerName = "Oath of Purging";

        public OathOfPurging()
        {
            Name = PowerName;
            StartingPower = true;
            ActiveText = "+2 dice in fights when attacking blights.";
            FulfillText = "Destroy a blight; you gain 1 Grace.";
            BreakText = "Enter the Monastery; you lose 1 Grace.";
        }

        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            hero.AddRollModifier(new OathOfPurgingModifier());
            hero.Triggers.Register(HeroTrigger.DestroyedBlight, new OathOfPurgingFulfilled {Name = Name});
            hero.Triggers.Register(HeroTrigger.LocationChanged, new OathOfPurgingBroken {Name = Name});
        }

        public override bool Deactivate(Hero hero)
        {
            if (!base.Deactivate(hero)) return false;
            hero.RemoveRollModifiers(Name);
            return true;
        }

        public override void Fulfill(Hero hero)
        {
            hero.GainGrace(1, int.MaxValue);
            Deactivate(hero);
        }

        public override void Break(Hero hero)
        {
            hero.LoseGrace();
            Deactivate(hero);
        }

        internal class OathOfPurgingModifier : IRollModifier
        {
            public string Name => PowerName;

            public int GetModifier(Hero hero, RollType rollType)
            {
                if (rollType != RollType.Fight) return 0;
                if (hero.ConflictState == null) return 0;
                if (hero.ConflictState.ConflictType != ConflictType.Attack) return 0;
                return 2;
            }

        }
        private class OathOfPurgingFulfilled : HeroTriggerHandler
        {

            public override void HandleTrigger(Hero hero, TriggerContext context)
            {
                var oath = (IOath)hero.GetPower(Name);
                oath.Fulfill(hero);
            }
        }

        private class OathOfPurgingBroken : HeroTriggerHandler
        {
            public override void HandleTrigger(Hero hero, TriggerContext context)
            {
                if (hero.Location != Location.Monastery) return;
                var oath = (IOath) hero.GetPower(Name);
                oath.Break(hero);
            }
        }
    }
}
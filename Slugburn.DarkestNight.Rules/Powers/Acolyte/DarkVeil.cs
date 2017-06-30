using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    class DarkVeil : BonusPower
    {
        private const string PowerName = "Dark Veil";

        public DarkVeil()
        {
            Name = PowerName;
            StartingPower = true;
            Text =
                "Exhaust at any time to ignore blights' effects until your next turn. *OR* Exhaust after you fail an attack on a blight to ignore its Defense.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddAction(new DarkVeilIgnoreEffects(this));
            hero.AddAction(new DarkVeilIgnoreDefense(this));
        }

        private class DarkVeilIgnoreEffects : PowerCommand
        {
            public DarkVeilIgnoreEffects(IPower power) : base("Dark Veil [ignore effects]", power)
            {
            }

            public override void Execute(Hero hero)
            {
                hero.Game.AddIgnoreBlight(IgnoreBlight.Create(Name, hero));
                hero.Triggers.Add(HeroTrigger.StartedTurn, Name, new DarkVeilEnds());
                _power.Exhaust(hero);
            }

            private class DarkVeilEnds : ITriggerHandler<Hero>
            {
                public void HandleTrigger(Hero registrar, string source, TriggerContext context)
                {
                    var hero = registrar;
                    hero.Game.RemoveIgnoreBlight(source);
                    hero.Triggers.Remove(HeroTrigger.StartedTurn, source);
                }
            }
        }

        private class DarkVeilIgnoreDefense : PowerCommand
        {
            public DarkVeilIgnoreDefense(IPower power) : base("Dark Veil [ignore defense]", power)
            {
            }

            public override void Execute(Hero hero)
            {
                var power = hero.GetPower(PowerName);
                if (!IsAvailable(hero))
                    throw new CommandNotAvailableException(hero, this);
                hero.ConflictState.SelectedTargets.First().IgnoreFailure();
                power.Exhaust(hero);
            }

            public override bool IsAvailable(Hero hero)
            {
                if (!base.IsAvailable(hero)) return false;
                if (hero.ConflictState == null) return false;
                if (!hero.ConflictState.SelectedTargets.Any()) return false;
                var target = hero.ConflictState.SelectedTargets.First();
                return target.ResultNumber != null && (target.Conflict is IBlight) && !target.IsWin;
            }
        }
    }
}
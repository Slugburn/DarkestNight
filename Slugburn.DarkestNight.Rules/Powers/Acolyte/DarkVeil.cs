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

        protected override void OnLearn()
        {
            Owner.AddAction(new DarkVeilIgnoreEffects(this));
            Owner.AddAction(new DarkVeilIgnoreDefense(this));
        }

        private class DarkVeilIgnoreEffects : PowerCommand, IBlightSupression, ITriggerHandler<Hero>
        {
            public DarkVeilIgnoreEffects(IPower power) : base("Dark Veil [ignore effects]", power, false)
            {
            }

            public override bool IsAvailable(Hero hero)
            {
                return base.IsAvailable(hero) && hero.IsTakingTurn && hero.State == HeroState.TakingTurn;
            }

            public override void Execute(Hero hero)
            {
                hero.Game.AddBlightSupression(this);
                hero.Triggers.Add(HeroTrigger.StartedTurn, Name, this);
                Power.Exhaust(hero);
            }

            public bool IsSupressed(IBlight blight, Hero hero = null)
            {
                return Power.Owner == hero;
            }

            public void HandleTrigger(Hero hero, string source, TriggerContext context)
            {
                hero.Game.RemoveBlightSupression(source);
                hero.Triggers.Remove(HeroTrigger.StartedTurn, source);
            }
        }

        private class DarkVeilIgnoreDefense : PowerCommand
        {
            public DarkVeilIgnoreDefense(IPower power) : base("Dark Veil [ignore defense]", power, false)
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
                var selectedTargets = hero.ConflictState?.SelectedTargets;
                if (selectedTargets == null) return false;
                if (!selectedTargets.Any()) return false;
                var target = selectedTargets.First();
                return target.ResultDie > 0 && (target.Conflict is IBlight) && !target.IsWin;
            }
        }
    }
}
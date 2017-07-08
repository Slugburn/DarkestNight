using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    internal class CallToDeath : ActionPower
    {
        private const string PowerName = "Call to Death";

        public CallToDeath()
        {
            Name = PowerName;
            Text =
                "Attack two blights in your location at once. Make a single fight roll with +1 die, then divide the dice between blights and resolve as two separate attacks (losing Secrecy for each).";
        }

        protected override void OnLearn()
        {
            Owner.AddAction(new CallToDeathAction(this));
        }


        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.GetBlights().Count() > 1;
        }

        private class CallToDeathAction : PowerAction
        {

            public CallToDeathAction(IActionPower power) : base(power)
            {
            }

            public override void Execute(Hero hero)
            {
                hero.AddModifier(StaticRollBonus.Create(Name, ModifierType.FightDice, 1));
                hero.SetRoll(RollBuilder.Create<CallToDeathRoll>());
                hero.ConflictState = new ConflictState
                {
                    ConflictType = ConflictType.Attack,
                    AvailableTactics = hero.GetAvailableFightTactics().GetInfo(hero),
                    AvailableTargets = hero.GetBlights().GetTargetInfo(hero.Game),
                    MinTarget = 2,
                    MaxTarget = 2
                };
                hero.DisplayConflictState();
            }
        }

        private class CallToDeathRoll : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                var roll = rollState.AdjustedRoll;
                var conflict = hero.ConflictState;
                conflict.Roll = roll;
                // default die assignments for most efficent use of rolls
                var bestDice = roll.OrderByDescending(x => x).Take(2).ToList();
                var bestDie = bestDice[0];
                var secondDie = bestDice[1];
                var prioritizedTargets = conflict.SelectedTargets.OrderByDescending(t => t.Conflict is Shroud ? 10 : t.TargetNumber).ToList();
                var harder = prioritizedTargets[0];
                var easier = prioritizedTargets[1];
                if (bestDie >= harder.TargetNumber || bestDie < easier.TargetNumber)
                {
                    harder.ResultDie = bestDie;
                    easier.ResultDie = secondDie;
                }
                else
                {
                    easier.ResultDie = bestDie;
                    harder.ResultDie = secondDie;
                }
                hero.DisplayConflictState();
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                hero.RemoveRollModifiers(PowerName);
                foreach (var target in hero.ConflictState.SelectedTargets)
                    hero.ResolveAttack(target);
                hero.ResolveCurrentConflict();
            }
        }
    }
}
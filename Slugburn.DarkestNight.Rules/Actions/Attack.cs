using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Attack : IAction
    {
        public string Name => "Attack";

        public void Act(Hero hero)
        {
            hero.ValidateState(HeroState.ChoosingAction);
            hero.SetRoll(RollBuilder.Create<AttackRoll>());
            var conflictState = new ConflictState
            {
                ConflictType = ConflictType.Attack,
                MinTarget = 1,
                MaxTarget = 1,
                AvailableTargets = hero.GetSpace().Blights.GetTargetInfo()
            };
            hero.ConflictState = conflictState;
            // hero.ConflictState.ConflictType needs to be set before calling hero.GetAvailableFightTactics()
            conflictState.AvailableTactics = hero.GetAvailableFightTactics().GetInfo(hero);
            hero.State = HeroState.SelectingTarget;
            hero.DisplayConflictState();
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsActionAvailable && hero.GetBlights().Any();
        }

        private class AttackRoll : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                hero.IsActionAvailable = false;
                rollState.TargetNumber = hero.ConflictState.SelectedTargets.Single().TargetNumber;
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                hero.ConflictState.Resolve(hero);
            }
        }
    }
}

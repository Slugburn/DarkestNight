using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Enemies;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class FightNecromancer : IAction
    {
        public const string ActionName = "Fight Necromancer";
        public string Name => ActionName;

        public void Act(Hero hero)
        {
            hero.ValidateState(HeroState.ChoosingAction);
            var necromancerTargetInfo = new [] {hero.Game.Necromancer}.GetTargetInfo();
            var conflictState = new ConflictState
            {
                ConflictType = ConflictType.Attack,
                SelectedTargets = necromancerTargetInfo,
                MinTarget = 1,
                MaxTarget = 1,
                AvailableTargets = necromancerTargetInfo
            };
            hero.ConflictState = conflictState;
            hero.SetRoll(RollBuilder.Create<FightNecromancerRoll>());
            // hero.ConflictState.ConflictType needs to be set before calling hero.GetAvailableFightTactics()
            conflictState.AvailableTactics = hero.GetAvailableFightTactics().GetInfo(hero);
            hero.State = HeroState.SelectingTarget;
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsActionAvailable && hero.GetBlights().Any();
        }

        private class FightNecromancerRoll : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                hero.IsActionAvailable = false;
                rollState.TargetNumber = hero.Game.Necromancer.Fight;
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                rollState.TargetNumber = hero.Game.Necromancer.Fight;
                var target = hero.ConflictState.SelectedTargets.Single();
                var assignment = TargetDieAssignment.Create(target.Id, rollState.Result);
                hero.State = HeroState.AssigningDice;
                hero.AssignDiceToTargets(new[] { assignment });
            }
        }

    }
}
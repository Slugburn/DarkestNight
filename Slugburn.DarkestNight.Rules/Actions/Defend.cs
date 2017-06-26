using System.Linq;
using Slugburn.DarkestNight.Rules.Enemies;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Defend : IAction
    {
        public string Name => "Defend";

        public void Act(Hero hero)
        {
            var enemies = hero.Enemies.Select(EnemyFactory.Create);
            hero.SetRoll(RollBuilder.Create<DefendRoll>());
            hero.ConflictState = new ConflictState
            {
                MaxTarget = 1,
                MinTarget = 1,
                AvailableTargets = enemies.GetTargetInfo(),
                AvailableTactics = hero.GetAvailableTactics().GetInfo(hero)
            };
            hero.State = HeroState.SelectingTarget;
            hero.DisplayConflictState();
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.Enemies != null && hero.Enemies.Any();
        }

        private class DefendRoll : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                var enemy = GetEnemy(hero);
                var tacticType = GetTacticType(hero);
                if (tacticType == TacticType.Elude)
                    hero.Triggers.Send(HeroTrigger.Eluding);
                UpdateTargetNumber(rollState, tacticType, enemy);
                hero.ConflictState.Roll = rollState.AdjustedRoll;
                hero.DisplayConflictState();
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                var target = hero.ConflictState.SelectedTargets.Single();
                target.ResultNumber = rollState.Result;
            }

            private static TacticType GetTacticType(Hero hero)
            {
                var tactic = hero.ConflictState.SelectedTactic;
                var tacticType = tactic.Type;
                return tacticType;
            }

            private static IEnemy GetEnemy(Hero hero)
            {
                var target = hero.ConflictState.SelectedTargets.Single();
                var enemy = EnemyFactory.Create(target.Name);
                return enemy;
            }

            private static void UpdateTargetNumber(RollState rollState, TacticType tacticType, IEnemy enemy)
            {
                var targetNumber = tacticType == TacticType.Fight ? enemy.Fight : enemy.Elude;
                rollState.TargetNumber = targetNumber;
            }
        }


    }
}

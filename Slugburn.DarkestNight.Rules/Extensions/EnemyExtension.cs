using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Enemies;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules
{
    public static class EnemyExtension
    {
        private static TargetInfo GetTargetInfo(this IEnemy enemy, int id)
        {
            return new TargetInfo(enemy)
            {
                Id = id,
                CanFight = enemy.Fight > 0,
                CanElude = enemy.Elude > 0,
                FightTarget = enemy.Fight,
                EludeTarget = enemy.Elude,
                Results = enemy.GetConflictResults().ToList()
            };
        }

        public static List<TargetInfo> GetTargetInfo(this IEnumerable<IEnemy> enemies)
        {
            return enemies.Select((enemy, id) => enemy.GetTargetInfo(id)).ToList();
        }

        public static IEnumerable<ConflictResult> GetConflictResults(this IConflict conflict)
        {
            var winFight = conflict.OutcomeDescription(true, TacticType.Fight);
            var winElude = conflict.OutcomeDescription(true, TacticType.Elude);
            var loseFight = conflict.OutcomeDescription(false, TacticType.Fight);
            var loseElude = conflict.OutcomeDescription(false, TacticType.Elude);
            var winResults = ConflictResults("Win", winFight, winElude);
            var failureResults = ConflictResults("Failure", loseFight, loseElude);
            return winResults.Concat(failureResults);
        }

        private static IEnumerable<ConflictResult> ConflictResults(string outcome, string fightEffect, string eludeEffect)
        {
            if (fightEffect == eludeEffect)
            {
                if (fightEffect != null)
                    yield return new ConflictResult(outcome, fightEffect);
            }
            else
            {
                if (fightEffect != null)
                    yield return new ConflictResult(outcome + " fight", fightEffect);
                if (eludeEffect != null)
                    yield return new ConflictResult(outcome + " elude", eludeEffect);
            }
        }
    }
}

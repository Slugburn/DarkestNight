using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Enemies;

namespace Slugburn.DarkestNight.Rules
{
    public static class BlightExtension
    {
        public static List<TargetInfo> GetTargetInfo(this IEnumerable<IBlight> blights, Game game)
        {
            return blights.Select(GetTargetInfo).ToList();
        }

        private static TargetInfo GetTargetInfo(this IBlight blight)
        {
            var targetInfo = new TargetInfo(blight)
            {
                Id = blight.Id,
                CanFight = true,
                FightTarget = blight.Might,
                Results = new List<ConflictResult>
                {
                    new ConflictResult("Win fight", "Destroy blight"),
                    new ConflictResult("Failure", blight.DefenseText)
                }
            };
            return targetInfo;
        }

        public static List<IEnemy> GenerateEnemies(this IEnumerable<IBlight> blights)
        {
            return blights.OfType<EnemyLair>().Select(x=>EnemyFactory.Create(x.EnemyName)).ToList();
        }
    }
}

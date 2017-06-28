using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Conflicts;

namespace Slugburn.DarkestNight.Rules.Enemies
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
                Results = enemy.GetResults().ToList()
            };
        }

        public static List<TargetInfo> GetTargetInfo(this IEnumerable<IEnemy> enemies)
        {
            return enemies.Select((enemy,id)=>enemy.GetTargetInfo(id)).ToList();
        }
    }
}

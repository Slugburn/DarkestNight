using System;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public class EnemyFactory
    {
        public static IEnemy Create(string name)
        {
            switch (name)
            {
                case "Archer":
                    return Enemy.Create(name, 4, 4);
                case "Cultist":
                    return new Cultist();
                case "Deadly Demon":
                    return Enemy.Create(name, 4, 5);
                case "Dread":
                    return Enemy.Create(name, 5, 5);
                case "Fearful Demon":
                    return new ScoutingEnemy(name, 3, 4, 2);
                case "Flying Demon":
                    return new ScoutingEnemy(name, 0, 6, 1);
                case "Ghoul":
                    return Enemy.Create(name, 4, 3);
                case "Giant Horde":
                    return Enemy.Create(name, 6, 5);
                case "Guarded Trove":
                    return new GuardedTrove();
                case "Hunter":
                    return Enemy.Create(name, 5, 6);
                case "Large Horde":
                    return Enemy.Create(name, 5, 4);
                case "Looters":
                    return new Looters();
                case "Lich":
                    return Enemy.Create(name, 5, 5);
                case "Mummy":
                    return Enemy.Create(name, 6, 4);
                case "Reaper":
                    return Enemy.Create(name, 6, 6);
                case "Revenant":
                    return Enemy.Create(name, 5, 4);
                case "Scout":
                    return new ScoutingEnemy(name, 0, 5, 1);
                case "Shade":
                    return Enemy.Create(name, 3, 5);
                case "Shadow":
                    return Enemy.Create(name, 4, 6);
                case "Skeleton":
                    return Enemy.Create(name, 4, 4);
                case "Slayer":
                    return Enemy.Create(name, 6, 5);
                case "Small Horde":
                    return Enemy.Create(name, 4, 3);
                case "Vampire":
                    return Enemy.Create(name, 4, 4);
                case "Vile Messenger":
                    return new VileMessenger();
                case "Zombie":
                    return Enemy.Create(name, 5, 3);
                default:
                    throw new ArgumentOutOfRangeException(nameof(name));
            }
        }
    }
}

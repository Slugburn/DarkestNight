using System;
using Slugburn.DarkestNight.Rules.Blights.Implementations;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public class BlightFactory
    {
        public IBlight Create(int id, BlightType blightType, Location location)
        {
            var blight = Create(blightType);
            blight.Id = id;
            blight.Location = location;
            return blight;
        }

        private static Blight Create(BlightType blightType)
        {
            switch (blightType)
            {
                case BlightType.Confusion:
                    return new Confusion();
                case BlightType.Corruption:
                    return new Corruption();
                case BlightType.Curse:
                    return new Curse();
                case BlightType.DarkFog:
                    return new DarkFog();
                case BlightType.Desecration:
                    return new Desecration();
                case BlightType.EvilPresence:
                    return new EvilPresence();
                case BlightType.Lich:
                    return new EnemyLair(BlightType.Lich, "Lich", 5, "Lich");
                case BlightType.Shades:
                    return new EnemyLair(BlightType.Shades, "Shades", 5, "Shade");
                case BlightType.Shroud:
                    return new Shroud();
                case BlightType.Skeletons:
                    return new EnemyLair(BlightType.Skeletons, "Skeletons", 5, "Skeleton");
                case BlightType.Spies:
                    return new Spies();
                case BlightType.Taint:
                    return new Taint();
                case BlightType.UnholyAura:
                    return new UnholyAura();
                case BlightType.Vampire:
                    return new EnemyLair(BlightType.Vampire, "Vampire", 6, "Vampire");
                case BlightType.Zombies:
                    return new EnemyLair(BlightType.Zombies, "Zombies", 5, "Zombie");
                default:
                    throw new ArgumentOutOfRangeException(nameof(blightType), blightType.ToString());
            }
        }
    }
}

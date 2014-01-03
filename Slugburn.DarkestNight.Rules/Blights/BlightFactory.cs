using System;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public class BlightFactory
    {
        public IBlight Create(Blight blight)
        {
            switch (blight)
            {
                case Blight.Confusion:
                    return new Confusion();
                case Blight.Corruption:
                    return new Corruption();
                case Blight.Curse:
                    return new Curse();
                case Blight.DarkFog:
                    return new DarkFog();
                case Blight.Desecration:
                    return new Desecration();
                case Blight.EvilPresence:
                    return new EvilPresence();
                case Blight.Lich:
                    return new Undead("Lich", 5, 5, 5);
                case Blight.Shades:
                    return new Undead("Shades", 5, 3, 5);
                case Blight.Shroud:
                    return new Shroud();
                case Blight.Skeletons:
                    return new Undead("Skeletons", 5, 4, 4);
                case Blight.Spies:
                    return new Spies();
                case Blight.Taint:
                    return new Taint();
                case Blight.UnholyAura:
                    return new UnholyAura();
                case Blight.Vampire:
                    return new Undead("Vampire", 6, 4, 4);
                case Blight.Zombies:
                    return new Undead("Zombies", 5, 5, 3);
                default:
                    throw new ArgumentOutOfRangeException("blight", blight, "Unknown blight type");
            }
        }
    }
}
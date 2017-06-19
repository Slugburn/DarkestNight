﻿using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights.Implementations;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public static class BlightExtension
    {
        private static readonly Blight[] UndeadList = {Blight.Lich, Blight.Shades, Blight.Skeletons, Blight.Vampire, Blight.Zombies};

        public static bool IsUndead(this Blight blight)
        {
            return UndeadList.Contains(blight);
        }

        public static IBlightDetail GetDetail(Blight blight)
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
                    return new Undead(Blight.Lich, "Lich", 5, "Lich");
                case Blight.Shades:
                    return new Undead(Blight.Shades, "Shades", 5, "Shade");
                case Blight.Shroud:
                    return new Shroud();
                case Blight.Skeletons:
                    return new Undead(Blight.Skeletons, "Skeletons", 5, "Skeleton");
                case Blight.Spies:
                    return new Spies();
                case Blight.Taint:
                    return new Taint();
                case Blight.UnholyAura:
                    return new UnholyAura();
                case Blight.Vampire:
                    return new Undead(Blight.Vampire, "Vampire", 6, "Vampire");
                case Blight.Zombies:
                    return new Undead(Blight.Zombies, "Zombies", 5, "Zombie");
                default:
                    throw new ArgumentOutOfRangeException(nameof(blight), blight, "Unknown blight type");
            }
        }

        public static List<TargetInfo> GetTargetInfo(this IEnumerable<Blight> blights)
        {
            return blights.Select((blight,id)=>blight.GetTargetInfo(id)).ToList();
        }

        private static TargetInfo GetTargetInfo(this Blight blight, int id)
        {
            var detail = GetDetail(blight);
            var targetInfo = new TargetInfo
            {
                Id = id,
                Name = detail.Name,
                CanFight = true,
                FightTarget = detail.Might,
                Results = new List<ConflictResult>
                {
                    new ConflictResult("Win fight", "Destroy blight"),
                    new ConflictResult("Failure", detail.DefenseText)
                }
            };
            return targetInfo;
        }
    }
}

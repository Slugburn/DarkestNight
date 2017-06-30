using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Maps
{
    public class MapFactory
    {
        public IEnumerable<IMap> CreateMaps()
        {
            return new IMap[]
            {
                new Map(new[] {BlightType.Shades, BlightType.Confusion, BlightType.UnholyAura, BlightType.Corruption, BlightType.DarkFog, BlightType.Desecration, BlightType.Spies},
                    new[] {Find.ForgottenShrine, Find.Key, Find.TreasureChest, Find.Key, Find.VanishingDust, Find.Key}),
                new Map(new[] {BlightType.Shroud, BlightType.UnholyAura, BlightType.Skeletons, BlightType.Confusion, BlightType.DarkFog, BlightType.EvilPresence, BlightType.Spies},
                    new[] {Find.Key, Find.VanishingDust, Find.TreasureChest, Find.Epiphany, Find.TreasureChest, Find.Key}),
                new Map(new[] {BlightType.Desecration, BlightType.EvilPresence, BlightType.Taint, BlightType.Lich, BlightType.Corruption, BlightType.UnholyAura, BlightType.Spies},
                    new[] {Find.TreasureChest, Find.TreasureChest, Find.TreasureChest, Find.Epiphany, Find.Key, Find.ForgottenShrine}),
                new Map(new[] {BlightType.Skeletons, BlightType.Shroud, BlightType.Corruption, BlightType.Shades, BlightType.Lich, BlightType.DarkFog, BlightType.Curse},
                    new[] {Find.SupplyCache, Find.SupplyCache, Find.SupplyCache, Find.BottledMagic, Find.Key, Find.Key}),
                new Map(new[] {BlightType.Desecration, BlightType.Vampire, BlightType.Confusion, BlightType.Skeletons, BlightType.Shades, BlightType.Lich, BlightType.Spies},
                    new[] {Find.SupplyCache, Find.SupplyCache, Find.Key, Find.Epiphany, Find.Waystone, Find.Key}),
                new Map(new[] {BlightType.Lich, BlightType.EvilPresence, BlightType.Spies, BlightType.Confusion, BlightType.Skeletons, BlightType.Shroud, BlightType.Desecration},
                    new[] {Find.Key, Find.Waystone, Find.TreasureChest, Find.BottledMagic, Find.SupplyCache, Find.SupplyCache}),
                new Map(new[] {BlightType.Zombies, BlightType.Spies, BlightType.Lich, BlightType.Desecration, BlightType.Shroud, BlightType.EvilPresence, BlightType.Taint},
                    new[] {Find.TreasureChest, Find.BottledMagic, Find.Key, Find.Artifact, Find.Artifact, Find.Key}),
                new Map(new[] {BlightType.Shroud, BlightType.Skeletons, BlightType.EvilPresence, BlightType.Shades, BlightType.UnholyAura, BlightType.Corruption, BlightType.Taint},
                    new[] {Find.Key, Find.SupplyCache, Find.SupplyCache, Find.Epiphany, Find.Key, Find.SupplyCache}),
                new Map(new[] {BlightType.Curse, BlightType.UnholyAura, BlightType.Spies, BlightType.Skeletons, BlightType.Confusion, BlightType.Vampire, BlightType.Desecration},
                    new[] {Find.TreasureChest, Find.ForgottenShrine, Find.SupplyCache, Find.Epiphany, Find.TreasureChest, Find.Key}),
                new Map(new[] {BlightType.Shades, BlightType.DarkFog, BlightType.Zombies, BlightType.Curse, BlightType.Desecration, BlightType.Spies, BlightType.UnholyAura},
                    new[] {Find.Key, Find.TreasureChest, Find.TreasureChest, Find.BottledMagic, Find.Key, Find.Key}),
                new Map(new[] {BlightType.EvilPresence, BlightType.Skeletons, BlightType.DarkFog, BlightType.Shroud, BlightType.Vampire, BlightType.UnholyAura, BlightType.Taint},
                    new[] {Find.Artifact, Find.Key, Find.SupplyCache, Find.VanishingDust, Find.TreasureChest, Find.Artifact}),
                new Map(new[] {BlightType.Taint, BlightType.Curse, BlightType.Spies, BlightType.Zombies, BlightType.DarkFog, BlightType.Skeletons, BlightType.Desecration},
                    new[] {Find.SupplyCache, Find.TreasureChest, Find.SupplyCache, Find.Waystone, Find.SupplyCache, Find.TreasureChest}),
                new Map(new[] {BlightType.Corruption, BlightType.Spies, BlightType.Curse, BlightType.Vampire, BlightType.Zombies, BlightType.Shroud, BlightType.UnholyAura},
                    new[] {Find.Key, Find.SupplyCache, Find.VanishingDust, Find.BottledMagic, Find.SupplyCache, Find.TreasureChest}),
                new Map(new[] {BlightType.Taint, BlightType.Curse, BlightType.Vampire, BlightType.Shades, BlightType.Zombies, BlightType.Spies, BlightType.Confusion},
                    new[] {Find.TreasureChest, Find.SupplyCache, Find.TreasureChest, Find.Epiphany, Find.Key, Find.Key}),
                new Map(new[] {BlightType.Vampire, BlightType.Desecration, BlightType.EvilPresence, BlightType.Zombies, BlightType.Confusion, BlightType.Shades, BlightType.Shroud},
                    new[] {Find.Waystone, Find.TreasureChest, Find.ForgottenShrine, Find.Epiphany, Find.TreasureChest, Find.Key}),
                new Map(new[] {BlightType.Confusion, BlightType.Spies, BlightType.Curse, BlightType.UnholyAura, BlightType.Shroud, BlightType.Zombies, BlightType.Corruption},
                    new[] {Find.TreasureChest, Find.TreasureChest, Find.Key, Find.Artifact, Find.TreasureChest, Find.Key}),
                new Map(new[] {BlightType.UnholyAura, BlightType.Shades, BlightType.Zombies, BlightType.Shroud, BlightType.Desecration, BlightType.Skeletons, BlightType.Curse},
                    new[] {Find.Key, Find.SupplyCache, Find.Artifact, Find.Artifact, Find.Key, Find.Key}),
                new Map(new[] {BlightType.Corruption, BlightType.DarkFog, BlightType.Zombies, BlightType.Taint, BlightType.Spies, BlightType.Skeletons, BlightType.Curse},
                    new[] {Find.Key, Find.TreasureChest, Find.BottledMagic, Find.TreasureChest, Find.Key, Find.Key}),
                new Map(new[] {BlightType.Shades, BlightType.Lich, BlightType.Curse, BlightType.Desecration, BlightType.EvilPresence, BlightType.Taint, BlightType.Spies},
                    new[] {Find.Key, Find.SupplyCache, Find.Key, Find.Epiphany, Find.Key, Find.Waystone}),
            };
        }
    }
}

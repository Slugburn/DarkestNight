using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules
{
    public enum Find
    {
        BottledMagic,
        Key,
        SupplyCache,
        TreasureChest,
        Waystone,
        ForgottenShrine,
        VanishingDust,
        Epiphany,
        Artifact
    }

    public static class FindExtension
    {
        private static readonly IDictionary<Find,string> FindToDescription = new Dictionary<Find,string>
        {
            { Find.BottledMagic, "Bottled Magic"},
            {Find.Key, "Key" },
            {Find.SupplyCache, "Supply Cache" },
            {Find.TreasureChest, "Treasure Chest" },
            {Find.Waystone, "Waystone" },
            {Find.ForgottenShrine, "Forgotten Shrine" },
            {Find.VanishingDust, "Vanishing Dust"},
            {Find.Epiphany, "Epiphany" },
            {Find.Artifact, "Artifact" }
        };

        private static readonly Dictionary<string, Find> DescriptionToFind;

        static FindExtension()
        {
            DescriptionToFind = FindToDescription.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        public static string ToDescription(this Find find)
        {
            return FindToDescription[find];
        }

        public static Find ToFind(this string description)
        {
            return DescriptionToFind[description];
        }
    }
}
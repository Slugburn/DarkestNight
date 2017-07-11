using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Slugburn.DarkestNight.Rules.Items;

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

        private static readonly IDictionary<Find, string> FindToText = new Dictionary<Find, string>
        {
            {Find.BottledMagic, new BottledMagic().Text},
            {Find.Key, new Key().Text},
            {Find.SupplyCache, "Draw two power cards; keep one, place the other at the bottom of your deck."},
            {Find.TreasureChest, new TreasureChest().Text},
            {Find.Waystone, new Waystone().Text},
            {Find.ForgottenShrine, "Gain 2 Grace."},
            {Find.VanishingDust, new VanishingDust().Text},
            {Find.Epiphany, "Search your power deck and take the card of your choice, then shuffle that deck."},
            {Find.Artifact, "Draw an artifact card."}
        };

        private static readonly Dictionary<string, Find> DescriptionToFind;

        static FindExtension()
        {
            DescriptionToFind = FindToDescription.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        public static string ToName(this Find find)
        {
            return FindToDescription[find];
        }

        public static Find ToFind(this string description)
        {
            return DescriptionToFind[description];
        }

        public static string ToText(this Find find)
        {
            return FindToText[find];
        }
    }
}
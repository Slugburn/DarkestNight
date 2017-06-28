namespace Slugburn.DarkestNight.Rules
{
    public enum Find
    {
        // Discard after a fight roll to add 3d to that roll.
        BottledMagic,

        // Three of these amy be discarded to retrieve a holy relic (as an action)
        Key,

        // Draw two power cards, keep one, place the other at the bottom of your deck
        SupplyCache,

        // Discard at any time to draw a new power card
        TreasureChest,

        // Discard during your turn to instantly move to any location and gain 1 Secrecy
        Waystone,

        // Gain 2 Grace
        ForgottenShrine,

        // Discard after a failed elusion roll to make it a success
        VanishingDust,

        // Search your power deck and take the card of your choice, then shuffle that deck
        Epiphany,

        // Draw an artifact card
        Artifact
    }
}
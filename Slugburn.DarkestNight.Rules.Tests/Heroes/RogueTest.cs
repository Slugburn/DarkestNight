using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class RogueTest
    {
        // Ambush (Tactic): Spend 1 Secrecy to fight with 3 dice.
        [Test]
        public void Ambush()
        {
            TestScenario.Game
                .WithHero("Rogue").HasPowers("Ambush").IsFacingEnemy("Skeleton")
                .Then(Verify.Player.ConflictModel.HasTactics("Ambush", "Fight", "Elude"))
                .When.Player.Targets("Skeleton").UsesTactic("Ambush").ResolvesConflict()
                .Then(Verify.Player.Hero("Rogue").LostSecrecy(1))
                .Then(Verify.Player.ConflictModel.Rolled(6, 6, 6));
        }

        [Test]
        public void Ambush_RequiresSecrecy()
        {
            TestScenario.Game
                .WithHero("Rogue").Secrecy(0).HasPowers("Ambush").IsFacingEnemy("Skeleton")
                .Then(Verify.Player.ConflictModel.HasTactics("Fight", "Elude"));
        }

        // Contacts (Bonus): Exhaust at any time to gain 1 Secrecy (up to 7).
        // Diversion (Action): Spend 1 Secrecy to negate the effects of one blight in your location until the Necromancer ends a turn there.
        // Eavesdrop (Action): Spend 1 Secrecy to search with 2 dice.
        // Sabotage (Action): Spend 1 Secrecy in the Necromancer's location to cause -1 Darkness.
        // Sap (Bonus): Exhaust during your turn to reduce the might of a blight in your location by 1 until your next turn.
        // Shadow Cloak (Bonus): +1 die when eluding.
        // Skulk (Tactic): Elude with 2 dice and add 1 to the highest die.
        // Stealth (Bonus): Any time you lose or spend Secrecy, you can spend 1 Grace instead.
        // Vanish (Tactic): Elude with 2 dice. Gain 1 Secrecy (up to 7) if you roll 2 successes.
    }
}

using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class ScholarTest
    {
        // Ancient Charm (Action): Activate in your location. 
        // When a hero has an event there, draw an extra card and discard 1.
        [Test]
        public void AncientCharm()
        {
            TestScenario.Game
                .WithHero("Scholar").HasPowers("Ancient Charm").At("Village")
                .Then(Verify.Player.Hero("Scholar").Commands.Includes("Ancient Charm"))
                .When.Player.TakesAction("Ancient Charm")
                .Then(Verify.Player.BoardView.Location("Village").Effects("Ancient Charm"));
        }

        // Ancient Defense (Action): Activate in your location. 
        // When a blight appears there, draw an extra card and discard 1.

        // Ancient Sword (Bonus): +1 die in fights.

        // Counterspell (Action): Activate in your location.
        // The might of blights there is reduced by 1.

        // Find Weakness (Tactic): Fight with 1 die. Before rolling, pick 1 die, and add 1 to its result.

        // Foresight (Tactic): Elude with 2 dice.

        // Forgotten Sanctuary (Action): Activate in your location.
        // Heroes gain +2 dice when eluding there.

        // Preparation (Bonus): Exhaust after you make any die roll to reroll it.

        // Research Materials (Bonus): +1 die in searches.

        // Thoroughness (Bonus): If you search successfully, draw an extra card (but still keep only 1).
    }
}

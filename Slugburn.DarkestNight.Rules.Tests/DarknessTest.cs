using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests
{
    [TestFixture]
    public class DarknessTest
    {
        [TestCase(1, "Mountains", 3)]
        [TestCase(2, "Ruins", 3)]
        [TestCase(3, "Forest", 2)]
        public void TwentyOrMore(int roll, string afterMove, int expectedCount)
        {
            // When darkness is 20+, necromancer creates an additional blight when he rolls a 1 or 2 for movement
            TestScenario.Game
                .Darkness(20)
                .Necromancer.At("Village")
                .When.Game.NecromancerActs(Fake.Rolls(roll))
                .When.Player.AcceptsNecromancerTurn()
                .Then(Verify.Player.BoardView.Location(afterMove).BlightCount(expectedCount));
        }

        [Test]
        public void TenOrMore()
        {
            // When darkness is 10+, necromancer creates two blights when there are no blights at his location
            TestScenario.Game
                .Darkness(10)
                .Necromancer.At("Village")
                .When.Game.NecromancerActs(Fake.Rolls(6)) // stays at village
                .When.Player.AcceptsNecromancerTurn()
                .Then(Verify.Player.BoardView.Location("Village").BlightCount(2));
        }
    }
}
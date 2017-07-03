using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests
{
    [TestFixture]
    public class NecromancerTest
    {
        [Test]
        public void IncreasesDarkness()
        {
            TestScenario.Game
                .Darkness(1)
                .When.Game.NecromancerActs()
                .Then(Verify.Player.BoardView.Darkness(2));
        }

        [TestCase("Castle",1, "Swamp")]
        [TestCase("Castle",2, "Village")]
        [TestCase("Castle",3, "Mountains")]
        [TestCase("Castle",4, "Castle")]
        [TestCase("Castle",5, "Village")]
        [TestCase("Castle",6, "Castle")]
        public void FollowsDieRollIfNoHeroesDetected(string start, int roll, string end)
        {
            TestScenario.Game
                .Darkness(0)
                .Necromancer.At(start)
                .When.Game.NecromancerActs(Fake.Rolls(roll))
                .Player.AcceptsNecromancerTurn()
                .Then(Verify.Player.BoardView.Location(end).Token("Necromancer"));
        }
    }
}

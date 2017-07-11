using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

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

        [Test]
        public void DefeatingNecromancerDestroysBlight()
        {
            TestScenario.Game
                .Necromancer.At("Ruins")
                .WithHero().At("Ruins").Secrecy(0).HasItems("Holy Relic").IsTakingTurn(false)
                .Location("Ruins").HasBlights("Skeletons")
                .When.Player.TakesAction("Start Turn").Fights()
                .Then(Verify.Location("Ruins").Blights());
        }

        [Test]
        public void DefeatingNecromancerDestroysBlight_MultipleBlights()
        {
            TestScenario.Game
                .Necromancer.At("Ruins")
                .WithHero().At("Ruins").Secrecy(0).HasItems("Holy Relic").IsTakingTurn(false)
                .Location("Ruins").HasBlights("Skeletons", "Vampire")
                .When.Player.TakesAction("Start Turn").Fights().SelectsBlight("Ruins", "Vampire")
                .Then(Verify.Location("Ruins").Blights("Skeletons"));
        }
    }
}

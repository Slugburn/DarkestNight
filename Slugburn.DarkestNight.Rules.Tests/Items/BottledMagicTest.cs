using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Items
{
    [TestFixture]
    public class BottledMagicTest 
    {
        // Discard after a fight roll to add 3d to that roll.
        [Test]
        public void UseIt()
        {
            TestScenario.Game
                .WithHero().HasItems("Bottled Magic").IsFacingEnemy("Zombie")
                .When.Player.Targets("Zombie").UsesTactic("Fight").ResolvesConflict(Fake.Rolls(1))
                .Then(Verify.Hero().CanTakeAction("Bottled Magic"))
                .When.Player.TakesAction("Bottled Magic", Fake.Rolls(4,5,6)).AcceptsRoll().AcceptsConflictResults()
                .Then(Verify.Hero().Rolled(1,4,5,6).HasItems());
        }

        [Test]
        public void DoesNotWorkOnElude()
        {
            TestScenario.Game
                .WithHero().HasItems("Bottled Magic").IsFacingEnemy("Zombie")
                .When.Player.Targets("Zombie").UsesTactic("Elude").ResolvesConflict(Fake.Rolls(1))
                .Then(Verify.Hero().CanTakeAction("Bottled Magic", false));
        }
    }
}

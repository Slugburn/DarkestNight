using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Items
{
    [TestFixture]
    public class VanishingDustTest 
    {
        // Discard after a fight roll to add 3d to that roll.
        [Test]
        public void UseIt()
        {
            TestScenario.Game
                .WithHero().HasItems("Vanishing Dust").IsFacingEnemy("Zombie")
                .When.Player.Targets("Zombie").UsesTactic("Elude").ResolvesConflict(Fake.Rolls(1))
                .Then(Verify.Hero().CanTakeAction("Vanishing Dust"))
                .When.Player.TakesAction("Vanishing Dust")
                .Then(Verify.Player.Hero().LostGrace(0))
                .Then(Verify.Player.ConflictModel.Rolled(1).Win())
                .Then(Verify.Hero().Rolled(6).HasItems());
        }

        [Test]
        public void AlreadyWinning()
        {
            TestScenario.Game
                .WithHero().HasItems("Vanishing Dust").IsFacingEnemy("Zombie")
                .When.Player.Targets("Zombie").UsesTactic("Elude").ResolvesConflict(Fake.Rolls(6))
                .Then(Verify.Hero().CanTakeAction("Vanishing Dust", false));
        }

        [Test]
        public void DoesNotWorkOnFight()
        {
            TestScenario.Game
                .WithHero().HasItems("Vanishing Dust").IsFacingEnemy("Zombie")
                .When.Player.Targets("Zombie").UsesTactic("Fight").ResolvesConflict(Fake.Rolls(1))
                .Then(Verify.Hero().CanTakeAction("Vanishing Dust", false));
        }
    }
}
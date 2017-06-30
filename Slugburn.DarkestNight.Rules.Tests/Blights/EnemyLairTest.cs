using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Blights
{
    [TestFixture]
    public class EnemyLairTest
    {
        [Test]
        public void MultipleLairs()
        {
            TestScenario.Game
                .WithHero().At("Swamp")
                .Location("Swamp").Blights("Lich", "Zombies")
                .When.Player.TakesAction("End Turn")
                .Then(Verify.Hero().HasUsedAction().IsTakingTurn().IsFacingEnemies("Lich", "Zombie"))
                .Then(Verify.Player.ConflictView.HasTargets("Lich", "Zombie"))
                .When.Player.Eludes("Zombie")
                .Then(Verify.Hero().HasUsedAction().IsTakingTurn().IsFacingEnemies("Lich"))
                .Then(Verify.Player.ConflictView.HasTargets("Lich"))
                .When.Player.Fights("Lich")
                .Then(Verify.Hero().HasUsedAction().IsTakingTurn(false));
        }
    }
}

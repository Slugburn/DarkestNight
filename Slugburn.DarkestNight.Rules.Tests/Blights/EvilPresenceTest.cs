using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Blights
{
    [TestFixture]
    public class EvilPresenceTest
    {
        [Test]
        public void Effect()
        {
            TestScenario.Game
                .WithHero("Priest").At("Village").HasPowers("Sanctuary")
                .Location("Village").HasBlights("EvilPresence", "EvilPresence")
                .Given.Hero().IsFacingEnemy("Zombie")
                .When.Player.UsesTactic("Sanctuary").Targets("Zombie").ResolvesConflict()
                .Then(Verify.Hero().RolledNumberOfDice(2));
        }

        [Test]
        public void Defense()
        {
            TestScenario.Game
                .WithHero("Priest").At("Village").HasPowers("Sanctuary")
                .Location("Village").HasBlights("EvilPresence")
                .When.Player.TakesAction("Attack").Fights(Fake.Rolls(1))
                .Then(Verify.Hero().HasUsedAction().HasUnresolvedEvents(1));
        }
    }
}

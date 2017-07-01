using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Blights
{
    [TestFixture]
    public class SpiesTest
    {
        [Test]
        public void Effect()
        {
            // At the end of each turn in the affected location, a hero loses 1 Secrecy.
            TestScenario.Game
                .WithHero().At("Village")
                .Location("Village").HasBlights("Spies")
                .When.Player.TakesAction("End Turn")
                .Then(Verify.Hero().LostSecrecy());
        }

        [Test]
        public void Defense()
        {
            // At the end of each turn in the affected location, a hero loses 1 Secrecy.
            TestScenario.Game
                .WithHero().At("Village")
                .Location("Village").HasBlights("Spies")
                .When.Player.TakesAction("Attack").Fights(Fake.Rolls(1))
                .Then(Verify.Hero().LostSecrecy(2)); // lose secrecy for both attacking and the Spies' defense
        }
    }
}

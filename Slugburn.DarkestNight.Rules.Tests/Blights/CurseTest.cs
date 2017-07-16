using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Blights
{
    [TestFixture]
    public class CurseTest
    {
        [Test]
        public void Effect()
        {
            // A hero that enters the affected location immediately loses 1 Grace.
            TestScenario.Game
                .WithHero().Location("Monastery")
                .Location("Village").HasBlights("Curse")
                .When.Player.TakesAction("Travel").SelectsDestination("Village")
                .Then(Verify.Hero().LostGrace().HasUsedAction());
        }

        [Test]
        public void Defense()
        {
            // Lose 1 Grace.
            TestScenario.Game
                .WithHero().At("Village")
                .Location("Village").HasBlights("Curse")
                .When.Player.TakesAction("Attack").Fights(Fake.Rolls(1))
                .Then(Verify.Hero().LostGrace().HasUsedAction());
        }
    }
}

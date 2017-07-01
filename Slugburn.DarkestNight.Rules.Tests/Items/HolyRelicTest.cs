using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Items
{
    [TestFixture]
    public class HolyRelicTest
    {
        // Each hero can only carry one at a time.
        [Test]
        public void CanOnlyCarryOne_CanNotRetrieveANewRelic()
        {
            // Each hero can only carry one at a time.
            TestScenario.Game
                .WithHero().HasItems("Holy Relic", "Key", "Key", "Key").At("Mountains")
                .Location("Mountains").HasRelic()
                .Then(Verify.Hero().CanTakeAction(RetrieveRelic.ActionName, false));
        }

        // Holy relics add 1 to your highest die in fights.
        // You lose 1 Secrecy at the start of your turn while carrying a holy relic.
        // You lose 1 Secrecy if another hero gives you a holy relic.
    }
}

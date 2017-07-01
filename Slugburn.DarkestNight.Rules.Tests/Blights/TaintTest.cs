using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Blights
{
    [TestFixture]
    public class TaintTest
    {
        [Test]
        public void Effect()
        {
            // While a hero is in the affected location, he cannot gain Grace. Whenever he would otherwise gain Grace, there is no effect.
            TestScenario.Game
                .WithHero().Grace(0).At("Village")
                .Location("Village").HasBlights("Taint")
                .NextSearchResult(Find.ForgottenShrine)
                .When.Player.CompletesSearch()
                .Then(Verify.Hero().Grace(0).CanGainGrace(false));
        }

        [Test]
        public void Defense()
        {
            // Lose 1 Grace.
            TestScenario.Game
                .WithHero().At("Village")
                .Location("Village").HasBlights("Taint")
                .When.Player.TakesAction("Attack").Fights(Fake.Rolls(1))
                .Then(Verify.Hero().LostGrace().CanGainGrace(false));
        }
    }
}

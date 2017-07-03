using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Items
{
    [TestFixture]
    public class StarryVeilTest
    {
        // When any hero at your location draws an event with a Fate of 5 or more, 
        // they may discard it and draw another.
        [Test]
        public void HappyPath()
        {
            TestScenario.Game
                .NextEvent("Anathema", "Betrayal", "Black Banner")
                .WithHero("Druid").At("Village").HasItems("Starry Veil")
                .WithHero("Priest").At("Village").IsTakingTurn().HasDrawnEvent()
                .Then(Verify.Player.EventView.HasTitle("Anathema").HasFate(6))
                .When.Player.TakesAction("Starry Veil")
                .Then(Verify.Player.EventView.HasTitle("Betrayal").HasFate(5))
                .When.Player.TakesAction("Starry Veil")
                .Then(Verify.Player.EventView.HasTitle("Black Banner").HasFate(4))
                .Then(Verify.Hero().HasUnresolvedEvents(1).CanTakeAction("Starry Veil", false));
        }

        [Test]
        public void NotUsableByHeroAtAnotherLocation()
        {
            TestScenario.Game
                .WithHero("Druid").At("Mountains").HasItems("Starry Veil")
                .WithHero("Priest").At("Village").IsTakingTurn().HasDrawnEvent("Betrayal")
                .Then(Verify.Player.Hero("Priest").Commands.None());
        }
    }
}
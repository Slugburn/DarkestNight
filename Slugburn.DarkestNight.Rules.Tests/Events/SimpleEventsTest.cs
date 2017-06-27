using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class SimpleEventsTest
    {
        [TestCase]
        public void Midnight()
        {
            TestScenario
                .Game.Darkness(3).WithHero()
                .When.Hero.DrawsEvent("Midnight")
                .Then(Verify.Player.EventView.HasBody("Midnight", 7, "+1 Darkness."))
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Game.Darkness(4));
        }

        [TestCase]
        public void Renewal()
        {
            TestScenario
                .Game.WithHero().DrawEvents(10)
                .When.Hero.DrawsEvent("Renewal")
                .Then(Verify.Player.EventView.HasBody("Renewal", 0, "Reshuffle the Event Deck and draw another card."))
                .Then(Verify.Hero.HasUnresolvedEvents(1).CurrentEvent.CanBeIgnored(false))
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Game.EventDeckIsReshuffled())
                .Then(Verify.Hero.HasUnresolvedEvents(1));
        }
    }
}
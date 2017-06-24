﻿using NUnit.Framework;
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
                .Given.Game.Darkness(3).WithHero()
                .When.Hero.DrawsEvent("Midnight")
                .When.Player.SelectsEventOption("Continue").Then().Game.Darkness(4);
        }

        [TestCase]
        public void Renewal()
        {
            TestScenario
                .Given.Game.WithHero().DrawEvents(10)
                .When.Hero.DrawsEvent("Renewal")
                .Then(Verify.Hero.HasUnresolvedEvents(1).CurrentEvent.CanBeIgnored(false))
                .When.Player.SelectsEventOption("Continue").Then().Game.EventDeckIsReshuffled()
                .Then(Verify.Hero.HasUnresolvedEvents(1));
        }
    }
}
﻿using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;

namespace Slugburn.DarkestNight.Rules.Tests.Actions
{
    [TestFixture]
    public class ActionTest
    {
        [TestCase("Travel")]
        [TestCase("Hide")]
        [TestCase("Attack")]
        [TestCase("Attack Necromancer")]
        [TestCase("Search")]
        [TestCase("Pray")]
        [TestCase("Retrieve a Holy Relic")]
        public void TakingTurnAndHasActionAvailable(string actionName)
        {
            TestScenario.Game
                .WithHero().IsTakingTurn()
                .Configure(given => ConfigureForAction(given, actionName))
                .Then(Verify.Hero().CanTakeAction(actionName).Grace(null));
        }

        [TestCase("Travel")]
        [TestCase("Hide")]
        [TestCase("Attack")]
        [TestCase("Attack Necromancer")]
        [TestCase("Search")]
        [TestCase("Pray")]
        [TestCase("Retrieve a Holy Relic")]
        public void NotTakingTurn(string actionName)
        {
            TestScenario.Game
                .WithHero().IsTakingTurn(false)
                .Configure(given => ConfigureForAction(given, actionName))
                .Then(Verify.Hero().CanTakeAction(actionName, false).Grace(null));
        }

        [TestCase("Travel")]
        [TestCase("Hide")]
        [TestCase("Attack")]
        [TestCase("Attack Necromancer")]
        [TestCase("Search")]
        [TestCase("Pray")]
        [TestCase("Retrieve a Holy Relic")]
        public void ActionAlreadyUsed(string actionName)
        {
            TestScenario.Game
                .WithHero().HasUsedAction()
                .Configure(given => ConfigureForAction(given, actionName))
                .Then(Verify.Hero().HasUsedAction().CanTakeAction(actionName, false).Grace(null));
        }

        [TestCase("Travel")]
        [TestCase("Hide")]
        [TestCase("Attack")]
        [TestCase("Attack Necromancer")]
        [TestCase("Search")]
        [TestCase("Pray")]
        [TestCase("Retrieve a Holy Relic")]
        public void ResolvingEvent(string actionName)
        {
            TestScenario.Game
                .WithHero().HasDrawnEvent()
                .Configure(given => ConfigureForAction(given, actionName))
                .Then(Verify.Hero().HasUnresolvedEvents(1).CanTakeAction(actionName, false).Grace(null));
        }

        private static IGiven ConfigureForAction(IGiven given, string actionName)
        {
            switch (actionName)
            {
                case "Attack":
                    return given.Location("Village").HasBlights("Desecration").Hero().At("Village");
                case "Attack Necromancer":
                    return given.Game.Necromancer.At("Village").Hero().At("Village");
                case "Hide":
                    return given.Hero().Secrecy(4);
                case "Pray":
                    return given.Hero().Grace(4);
                case "Search":
                    return given.Hero().At("Mountains");
                case "Retrieve a Holy Relic":
                    return given.Hero().At("Mountains").HasItems("Key", "Key", "Key");
            }
            return given;
        }
    }
}

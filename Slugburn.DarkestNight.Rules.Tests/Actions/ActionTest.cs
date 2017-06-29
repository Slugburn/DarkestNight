using NUnit.Framework;
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
        [TestCase("Search")]
        [TestCase("Pray")]
        [TestCase("Retrieve a Holy Relic")]
        public void TakingTurnAndHasActionAvailable(string actionName)
        {
            TestScenario.Game
                .WithHero().IsTakingTurn()
                .Configure(given => ConfigureForAction(given, actionName))
                .Then(Verify.Hero().CanTakeAction(actionName));
        }

        [TestCase("Travel")]
        [TestCase("Hide")]
        [TestCase("Attack")]
        [TestCase("Search")]
        [TestCase("Pray")]
        [TestCase("Retrieve a Holy Relic")]
        public void NotTakingTurn(string actionName)
        {
            TestScenario.Game
                .WithHero().IsTakingTurn(false)
                .Configure(given => ConfigureForAction(given, actionName))
                .Then(Verify.Hero().CanTakeAction(actionName, false));
        }

        [TestCase("Travel")]
        [TestCase("Hide")]
        [TestCase("Attack")]
        [TestCase("Search")]
        [TestCase("Pray")]
        [TestCase("Retrieve a Holy Relic")]
        public void ActionAlreadyUsed(string actionName)
        {
            TestScenario.Game
                .WithHero().HasTakenAction()
                .Configure(given => ConfigureForAction(given, actionName))
                .Then(Verify.Hero().HasUsedAction().CanTakeAction(actionName, false));
        }

        private static IGiven ConfigureForAction(IGiven given, string actionName)
        {
            switch (actionName)
            {
                case "Attack":
                    return given.Location("Village").Blights("Desecration").Hero().At("Village");
                case "Hide":
                    return given.Hero().Secrecy(4);
                case "Search":
                    return given.Hero().At("Mountains");
                case "Retrieve a Holy Relic":
                    return given.Hero().At("Mountains").HasItems("Key", "Key", "Key");
            }
            return given;
        }
    }
}

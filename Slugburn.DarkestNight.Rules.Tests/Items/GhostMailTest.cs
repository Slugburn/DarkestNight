using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Items
{
    // At the start of your turn, you may spend 1 Grace to gain 1 Secrecy (up to default),
    // or spend 1 Secrecy to gain 1 Grace (up to default).
    [TestFixture]
    public class GhostMailTest
    {
        [Test]
        public void GhostMail_SpendGrace()
        {
            TestScenario.Game
                .WithHero().HasItems("Ghost Mail").Secrecy(0).IsTakingTurn(false)
                .When.Player.TakesAction("Start Turn")
                .Then(Verify.Player.Hero().Commands.Exactly("Ghost Mail", "Continue"))
                .When.Player.TakesAction("Ghost Mail")
                .When.Player.AnswersQuestion("Ghost Mail", "Spend Grace")
                .Then(Verify.Player.Hero().Secrecy(1));
        }

        [Test]
        public void GhostMail_SpendSecrecy()
        {
            TestScenario.Game
                .WithHero().HasItems("Ghost Mail").Grace(0).IsTakingTurn(false)
                .When.Player.TakesAction("Start Turn")
                .Then(Verify.Player.Hero().Commands.Exactly("Ghost Mail", "Continue"))
                .When.Player.TakesAction("Ghost Mail")
                .When.Player.AnswersQuestion("Ghost Mail", "Spend Secrecy")
                .Then(Verify.Player.Hero().Grace(1));
        }
    }
}
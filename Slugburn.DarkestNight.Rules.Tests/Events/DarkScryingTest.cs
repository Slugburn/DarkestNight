﻿using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class DarkScryingTest
    {
        [TestCase("Spend Grace", 1, 0)]
        [TestCase("Lose Secrecy", 0, 2)]
        public void CloseCall_GraceAvailable(string option, int lostGrace, int lostSecrecy)
        {
            TestScenario
                .Game.WithHero("Acolyte")
                .Given.Hero().HasDrawnEvent("Dark Scrying")
                .Then(Verify.Player.EventView.HasBody("Dark Scrying", 4, "Spend 1 Grace or lose 2 Secrecy.").HasOptions("Spend Grace", "Lose Secrecy"))
                .When.Player.SelectsEventOption(option)
                .Then(Verify.Hero().LostSecrecy(lostSecrecy).LostGrace(lostGrace));
        }

        [Test]
        public void CloseCall_NoGraceAvailable()
        {
            TestScenario
                .Game.WithHero("Acolyte").Grace(0)
                .Given.Hero().HasDrawnEvent("Dark Scrying")
                .Then(Verify.Player.EventView.HasBody("Dark Scrying", 4, "Spend 1 Grace or lose 2 Secrecy.").HasOptions("Lose Secrecy"))
                .When.Player.SelectsEventOption("Lose Secrecy")
                .Then(Verify.Hero().Grace(0).LostSecrecy(2));
        }
    }
}
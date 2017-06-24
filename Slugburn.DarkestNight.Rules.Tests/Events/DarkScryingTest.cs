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
                .Given.Game.Hero("Acolyte")
                .When.Hero.DrawsEvent("Dark Scrying")
                .Then.Player.Event.HasBody("Dark Scrying", 4, "Spend 1 Grace or lose 2 Secrecy.").HasOptions("Spend Grace", "Lose Secrecy")
                .When.Player.SelectsEventOption(option)
                .Then.Hero(h => h.LostSecrecy(lostSecrecy).LostGrace(lostGrace));
        }

        [Test]
        public void CloseCall_NoGraceAvailable()
        {
            TestScenario
                .Given.Game.Hero("Acolyte", x => x.Grace(0))
                .When.Hero.DrawsEvent("Dark Scrying")
                .Then.Player.Event.HasBody("Dark Scrying", 4, "Spend 1 Grace or lose 2 Secrecy.").HasOptions("Lose Secrecy")
                .When.Player.SelectsEventOption("Lose Secrecy")
                .Then.Hero(h => h.Grace(0).LostSecrecy(2));
        }
    }
}
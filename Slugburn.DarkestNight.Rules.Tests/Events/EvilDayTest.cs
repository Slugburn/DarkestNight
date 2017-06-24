using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class EvilDayTest
    {
        [Test]
        public void EvilDay_ExhaustAPower()
        {
            TestScenario
                .Given.Game.Hero("Acolyte", x => x.HasPowers("Blinding Black", "Dark Veil", "False Life"))
                .Given.ActingHero(h => h.Power("Dark Veil", x => x.IsExhausted()))
                .When.Hero(x => x.DrawsEvent("Evil Day"))
                .Then.Player(p => p.Event(e => e
                    .HasBody("Evil Day", 5, "Exhaust a power or draw 2 more events.")
                    .HasOptions("Exhaust Power", "Draw Events")))
                .When.Player().SelectsEventOption("Exhaust Power")
                .Then.Player(p => p.Powers("Blinding Black", "False Life"))
                .When.Player().SelectsPower("False Life")
                .Then.Hero(h => h.Power("False Life", x => x.IsExhausted()));
        }

        [Test]
        public void EvilDay_DrawEvents()
        {
            TestScenario
                .Given.Game.Hero("Acolyte", x => x.HasPowers("Blinding Black", "Dark Veil", "False Life"))
                .When.Hero(x => x.DrawsEvent("Evil Day"))
                .Then.Player(p => p.Event(e => e
                    .HasBody("Evil Day", 5, "Exhaust a power or draw 2 more events.")
                    .HasOptions("Exhaust Power", "Draw Events")))
                .When.Player().SelectsEventOption("Draw Events")
                .Then.Hero(h => h.Event(e => e.HasOutstanding(2)));
        }

        [Test]
        public void EvilDay_NoPowerToExhaust()
        {
            TestScenario
                .Given.Game.Hero("Acolyte", x => x.HasPowers())
                .When.Hero(x => x.DrawsEvent("Evil Day"))
                .Then.Player(p => p.Event(e => e.HasOptions("Draw Events")));
        }
    }
}

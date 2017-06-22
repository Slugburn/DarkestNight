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
            new TestScenario()
                .GivenHero("Acolyte", x => x.Power("Blinding Black", "Dark Veil", "False Life"))
                .GivenPower("Dark Veil", x => x.IsExhausted())
                .WhenHero(x => x.DrawsEvent("Evil Day"))
                .ThenPlayer(p => p.Event(e => e
                    .HasBody("Evil Day", 5, "Exhaust a power or draw 2 more events.")
                    .HasOptions("Exhaust Power", "Draw Events")))
                .WhenPlayer(p => p.SelectsEventOption("Exhaust Power"))
                .ThenPlayer(p => p.Powers("Blinding Black", "False Life"))
                .WhenPlayer(p => p.SelectsPower("False Life"))
                .ThenPower("False Life", x => x.IsExhausted());
        }

        [Test]
        public void EvilDay_DrawEvents()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.Power("Blinding Black", "Dark Veil", "False Life"))
                .WhenHero(x => x.DrawsEvent("Evil Day"))
                .ThenPlayer(p => p.Event(e => e
                    .HasBody("Evil Day", 5, "Exhaust a power or draw 2 more events.")
                    .HasOptions("Exhaust Power", "Draw Events")))
                .WhenPlayer(p => p.SelectsEventOption("Draw Events"))
                .ThenHero(h=>h.HasOutstandingEvents(2));
        }

        [Test]
        public void EvilDay_NoPowerToExhaust()
        {
            new TestScenario()
                .GivenHero("Acolyte", x=>x.Power())
                .WhenHero(x => x.DrawsEvent("Evil Day"))
                .ThenPlayer(p => p.Event(e => e.HasOptions("Draw Events")));
        }
    }
}

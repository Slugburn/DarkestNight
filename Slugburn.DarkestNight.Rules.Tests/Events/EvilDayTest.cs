using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class EvilDayTest
    {
        [Test]
        public void EvilDay_DrawEvents()
        {
            TestScenario
                .Given.Game.WithHero("Acolyte", x => x.HasPowers("Blinding Black", "Dark Veil", "False Life"))
                .When.Hero.DrawsEvent("Evil Day").Then().Player.EventView
                .HasBody("Evil Day", 5, "Exhaust a power or draw 2 more events.")
                .HasOptions("Exhaust Power", "Draw Events")
                .When.Player.SelectsEventOption("Draw Events")
                .Then(Verify.Hero.HasUnresolvedEvents(2));
        }

        [Test]
        public void EvilDay_ExhaustAPower()
        {
            TestScenario
                .Given.Game.WithHero("Acolyte", x => x.HasPowers("Blinding Black", "Dark Veil", "False Life"))
                .Given.ActingHero(h => h.Power("Dark Veil", x => x.IsExhausted()))
                .When.Hero.DrawsEvent("Evil Day")
                .Then().Player.EventView.HasBody("Evil Day", 5, "Exhaust a power or draw 2 more events.").HasOptions("Exhaust Power", "Draw Events")
                .When.Player.SelectsEventOption("Exhaust Power")
                .Then().Player.PowerSelectionView("Blinding Black", "False Life")
                .When.Player.SelectsPower("False Life")
                .Then(Verify.Power("False Life").IsExhausted());
        }

        [Test]
        public void EvilDay_NoPowerToExhaust()
        {
            TestScenario
                .Given.Game.WithHero("Acolyte", x => x.HasPowers())
                .When.Hero.DrawsEvent("Evil Day")
                .Then().Player.EventView.HasOptions("Draw Events");
        }
    }
}
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class LatentSpellTest
    {
        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public void LatentSpell_NoEffect(int roll)
        {
            TestScenario
                .Game.WithHero().At("Ruins")
                .Given.ActingHero().DrawsEvent("Latent Spell")
                .When.Player.SelectsEventOption("Spend Grace", Fake.Rolls(roll))
                .Then(Verify.Player.EventView.ActiveRow("No effect"))
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Hero.LostSecrecy().LostGrace());
        }

        [Test]
        public void LatentSpell_ChooseDiscardEvent()
        {
            TestScenario
                .Game.WithHero()
                .Given.ActingHero().DrawsEvent("Latent Spell")
                .Then(Verify.Player.EventView.HasOptions("Spend Grace", "Discard Event"))
                .When.Player.SelectsEventOption("Discard Event")
                .Then(Verify.Hero.LostSecrecy());
        }

        [Test]
        public void LatentSpell_DestroyBlight()
        {
            TestScenario
                .Game.WithHero()
                .Given.ActingHero().DrawsEvent("Latent Spell")
                .When.Player.SelectsEventOption("Spend Grace", Fake.Rolls(6))
                .Then(Verify.Player.EventView.ActiveRow("Destroy a blight of your choice anywhere on the board"))
                .Given.Location("Village").Blights("Confusion", "Vampire")
                .Given.Location("Castle").Blights("Desecration")
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Player.BlightSelectionView.Location("Village").WithBlights("Confusion", "Vampire").Location("Castle").WithBlights("Desecration"))
                .When.Player.SelectsBlight("Village", "Vampire")
                .Then(Verify.Location("Village").Blights("Confusion"))
                .Then(Verify.Hero.LostSecrecy().LostGrace());
        }

        [Test]
        public void LatentSpell_DrawPower()
        {
            TestScenario
                .Game.WithHero("Acolyte")
                .Given.ActingHero().DrawsEvent("Latent Spell")
                .When.Player.SelectsEventOption("Spend Grace", Fake.Rolls(5))
                .Then(Verify.Player.EventView.ActiveRow("Draw a power card"))
                .Given.ActingHero().PowerDeck("Leech Life")
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Player.PowerSelectionView("Leech Life"))
                .Then(Verify.Hero.LostSecrecy().LostGrace().HasPowers("Leech Life"));
        }

        [Test]
        public void LatentSpell_Move()
        {
            TestScenario
                .Game.WithHero().At("Ruins")
                .Given.ActingHero().DrawsEvent("Latent Spell")
                .When.Player.SelectsEventOption("Spend Grace", Fake.Rolls(4))
                .Then(Verify.Player.EventView.ActiveRow("Move to any other location"))
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Player.LocationSelectionView("Monastery", "Mountains", "Castle", "Swamp", "Village", "Forest"))
                .When.Player.SelectsLocation("Monastery")
                .Then(Verify.Hero.LostSecrecy().LostGrace().Location("Monastery"));
        }

        [Test]
        public void LatentSpell_NoGrace()
        {
            TestScenario
                .Game.WithHero().Grace(0)
                .Given.ActingHero().DrawsEvent("Latent Spell")
                .Then(Verify.Player.EventView
                    .HasBody("Latent Spell", 2,
                        "Lose 1 Secrecy. Then, spend 1 Grace or discard this event without further effect.\nRoll 1d and take the highest")
                    .HasOptions("Discard Event"))
                .When.Player.SelectsEventOption("Discard Event")
                .Then(Verify.Hero.LostSecrecy().Grace(0));
        }
    }
}
﻿using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
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
                .Given.Game.Hero(x => x.Location("Ruins"))
                .When.Hero.DrawsEvent("Latent Spell")
                .When.Player.SelectsEventOption("Spend Grace", x => x.Rolls(roll))
                .Then.Player.Event.ActiveRow("No effect")
                .When.Player.SelectsEventOption("Continue")
                .Then.Hero(h => h.LostSecrecy().LostGrace());
        }

        [Test]
        public void LatentSpell_ChooseDiscardEvent()
        {
            TestScenario
                .Given.Game.Hero()
                .When.Hero.DrawsEvent("Latent Spell")
                .Then.Player.Event.HasOptions("Spend Grace", "Discard Event")
                .When.Player.SelectsEventOption("Discard Event")
                .Then.Hero(h => h.LostSecrecy());
        }

        [Test]
        public void LatentSpell_DestroyBlight()
        {
            TestScenario
                .Given.Game.Hero()
                .When.Hero.DrawsEvent("Latent Spell")
                .When.Player.SelectsEventOption("Spend Grace", x => x.Rolls(6))
                .Then.Player.Event.ActiveRow("Destroy a blight of your choice anywhere on the board")
                .Given.Location("Village", s => s.Blight("Confusion", "Vampire"))
                .Given.Location("Castle", s => s.Blight("Desecration"))
                .When.Player.SelectsEventOption("Continue")
                .Then.Player.Blights(b => b.Location("Village", "Confusion", "Vampire").Location("Castle", "Desecration"))
                .When.Player.SelectsBlight("Village", "Vampire")
                .Then.Location("Village", l => l.Blights("Confusion"))
                .Then.Hero(h => h.LostSecrecy().LostGrace());
        }

        [Test]
        public void LatentSpell_DrawPower()
        {
            TestScenario
                .Given.Game.Hero("Acolyte")
                .When.Hero.DrawsEvent("Latent Spell")
                .When.Player.SelectsEventOption("Spend Grace", x => x.Rolls(5))
                .Then.Player.Event.ActiveRow("Draw a power card")
                .Given.ActingHero(h => h.PowerDeck("Leech Life"))
                .When.Player.SelectsEventOption("Continue")
                .Then.Player.Powers("Leech Life")
                .Then.Hero(p => p.LostSecrecy().LostGrace().Powers("Leech Life"));
        }

        [Test]
        public void LatentSpell_Move()
        {
            TestScenario
                .Given.Game.Hero(x => x.Location("Ruins"))
                .When.Hero.DrawsEvent("Latent Spell")
                .When.Player.SelectsEventOption("Spend Grace", x => x.Rolls(4))
                .Then.Player.Event.ActiveRow("Move to any other location")
                .When.Player.SelectsEventOption("Continue")
                .Then.Player.SelectingLocation("Monastery", "Mountains", "Castle", "Swamp", "Village", "Forest")
                .When.Player.SelectsLocation("Monastery")
                .Then.Hero(h => h.LostSecrecy().LostGrace().Location("Monastery"));
        }

        [Test]
        public void LatentSpell_NoGrace()
        {
            TestScenario
                .Given.Game.Hero(h => h.Grace(0))
                .When.Hero.DrawsEvent("Latent Spell")
                .Then.Player.Event.HasBody("Latent Spell", 2,
                    "Lose 1 Secrecy. Then, spend 1 Grace or discard this event without further effect.\nRoll 1d and take the highest")
                .HasOptions("Discard Event")
                .When.Player.SelectsEventOption("Discard Event")
                .Then.Hero(h => h.LostSecrecy().Grace(0));
        }
    }
}
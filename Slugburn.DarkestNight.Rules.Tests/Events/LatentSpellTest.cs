using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class LatentSpellTest
    {
        [Test]
        public void LatentSpell_NoGrace()
        {
            new TestScenario()
                .GivenHero(h => h.Grace(0))
                .WhenHero(h => h.DrawsEvent("Latent Spell"))
                .ThenPlayer(p => p.Event(e => e.HasBody("Latent Spell", 2,
                    "Lose 1 Secrecy. Then, spend 1 Grace or discard this event without further effect.\nRoll 1d and take the highest")
                    .HasOptions("Discard Event")))
                .WhenPlayer(x => x.SelectsEventOption("Discard Event"))
                .ThenHero(h => h.LostSecrecy().Grace(0));
        }

        [Test]
        public void LatentSpell_ChooseDiscardEvent()
        {
            new TestScenario()
                .GivenHero()
                .WhenHero(h => h.DrawsEvent("Latent Spell"))
                .ThenPlayer(p => p.Event(e => e.HasOptions("Spend Grace", "Discard Event")))
                .WhenPlayer(x => x.SelectsEventOption("Discard Event"))
                .ThenHero(h => h.LostSecrecy());
        }

        [Test]
        public void LatentSpell_DestroyBlight()
        {
            new TestScenario()
                .GivenHero()
                .WhenHero(h => h.DrawsEvent("Latent Spell"))
                .WhenPlayer(p => p.SelectsEventOption("Spend Grace", x => x.Rolls(6)))
                .ThenPlayer(p => p.Event(e => e.ActiveRow(6, "Destroy a blight of your choice anywhere on the board")))
                .GivenLocation("Village", s => s.Blight("Confusion", "Vampire"))
                .GivenLocation("Castle", s => s.Blight("Desecration"))
                .WhenPlayer(p => p.SelectsEventOption("Continue"))
                .ThenPlayer(p => p.Blights(b => b.Location("Village", "Confusion", "Vampire").Location("Castle", "Desecration")))
                .WhenPlayer(p => p.SelectsBlight("Village", "Vampire"))
                .ThenSpace("Village", l => l.Blights("Confusion"))
                .ThenHero(h => h.LostSecrecy().LostGrace());
        }

        [Test]
        public void LatentSpell_DrawPower()
        {
            new TestScenario()
                .GivenHero("Acolyte")
                .WhenHero(h => h.DrawsEvent("Latent Spell"))
                .WhenPlayer(p => p.SelectsEventOption("Spend Grace", x => x.Rolls(5)))
                .ThenPlayer(p => p.Event(e => e.ActiveRow(5, "Draw a power card")))
                .GivenActingHero(h => h.PowerDeck("Leech Life"))
                .WhenPlayer(p => p.SelectsEventOption("Continue"))
                .ThenPlayer(p => p.Powers("Leech Life"))
                .ThenHero(p => p.LostSecrecy().LostGrace().Powers("Leech Life"));
        }

        [Test]
        public void LatentSpell_Move()
        {
            new TestScenario()
                .GivenHero(x => x.Location("Ruins"))
                .WhenHero(h => h.DrawsEvent("Latent Spell"))
                .WhenPlayer(p => p.SelectsEventOption("Spend Grace", x => x.Rolls(4)))
                .ThenPlayer(p => p.Event(e => e.ActiveRow(4, "Move to any other location")))
                .WhenPlayer(p => p.SelectsEventOption("Continue"))
                .ThenPlayer(p => p.SelectingLocation("Monastery", "Mountains", "Castle", "Swamp", "Village", "Forest"))
                .WhenPlayer(p => p.SelectsLocation("Monastery"))
                .ThenHero(h => h.LostSecrecy().LostGrace().Location("Monastery"));
        }

        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public void LatentSpell_NoEffect(int roll)
        {
            new TestScenario()
                .GivenHero(x => x.Location("Ruins"))
                .WhenHero(h => h.DrawsEvent("Latent Spell"))
                .WhenPlayer(p => p.SelectsEventOption("Spend Grace", x => x.Rolls(roll)))
                .ThenPlayer(p => p.Event(e => e.ActiveRow(1, 3, "No effect")))
                .WhenPlayer(p => p.SelectsEventOption("Continue"))
                .ThenHero(h => h.LostSecrecy().LostGrace());
        }
    }
}

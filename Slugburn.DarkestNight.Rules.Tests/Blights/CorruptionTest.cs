using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Blights
{
    [TestFixture]
    public class CorruptionTest
    {
        // While a hero is in the affected location, his Bonus powers have no effect.
        [Test]
        public void Effect_BonusDiceSupressed()
        {
            TestScenario.Game
                .WithHero("Knight").At("Monastery").HasPowers("Consecrated Blade")
                .Location("Monastery").HasBlights("Corruption")
                .Then(Verify.Hero().FightDice(1));
        }

        [Test]
        public void Effect_BonusEffectsSupressed()
        {
            TestScenario.Game
                .WithHero("Knight").At("Monastery").Grace(0).HasPowers("Holy Mantle")
                .Location("Monastery").HasBlights("Corruption")
                .When.Player.TakesAction("Pray", Fake.Rolls(2, 2)).AcceptsRoll()
                .Then(Verify.Hero().HasUsedAction().DefaultGrace(5).Grace(0));
        }

        [Test]
        public void Effect_BonusActionsSupressed()
        {
            TestScenario.Game
                .WithHero("Acolyte").At("Village").Grace(0).HasPowers("Dark Veil")
                .Location("Village").HasBlights("Corruption")
                .Then(Verify.Hero().CanTakeAction("Dark Veil", false));
        }

        [Test]
        public void Defense()
        {
            // Exhaust all powers.
            TestScenario.Game
                .WithHero("Druid").At("Village").HasPowers("Sprite Form", "Tranquility", "Wolf Form")
                .Location("Village").HasBlights("Corruption")
                .When.Player.TakesAction("Attack").Fights(Fake.Rolls(1))
                .Then(Verify.Hero().HasUsedAction())
                .Then(Verify.Power("Sprite Form").IsExhausted())
                .Then(Verify.Power("Tranquility").IsExhausted())
                .Then(Verify.Power("Wolf Form").IsExhausted());
        }
    }
}

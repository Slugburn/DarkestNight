using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class RogueTest
    {
        // Ambush (Tactic): Spend 1 Secrecy to fight with 3 dice.
        [Test]
        public void Ambush()
        {
            TestScenario.Game
                .WithHero("Rogue").HasPowers("Ambush").IsFacingEnemy("Skeleton")
                .Then(Verify.Player.ConflictModel.HasTactics("Ambush", "Fight", "Elude"))
                .When.Player.Targets("Skeleton").UsesTactic("Ambush").ResolvesConflict()
                .Then(Verify.Player.Hero("Rogue").LostSecrecy(1))
                .Then(Verify.Player.ConflictModel.Rolled(6, 6, 6));
        }

        [Test]
        public void Ambush_RequiresSecrecy()
        {
            TestScenario.Game
                .WithHero("Rogue").Secrecy(0).HasPowers("Ambush").IsFacingEnemy("Skeleton")
                .Then(Verify.Player.ConflictModel.HasTactics("Fight", "Elude"));
        }

        // Contacts (Bonus): Exhaust at any time to gain 1 Secrecy (up to 7).
        [Test]
        public void Contacts()
        {
            TestScenario.Game
                .WithHero("Rogue").Secrecy(0).HasPowers("Contacts")
                .When.Player.TakesAction("Contacts")
                .Then(Verify.Power("Contacts").IsExhausted())
                .Then(Verify.Player.Hero("Rogue").Secrecy(1));
        }

        [Test]
        public void Contacts_NotUsuableAtMaxSecrecy()
        {
            TestScenario.Game
                .WithHero("Rogue").Secrecy(7).HasPowers("Contacts")
                .Then(Verify.Player.Hero("Rogue").Commands.Excludes("Contacts"));
        }

        // Diversion (Action): Spend 1 Secrecy to negate the effects of one blight in your location until the Necromancer ends a turn there.
        [Test]
        public void Diversion()
        {
            TestScenario.Game
                .Location("Village").HasBlights("Desecration", "Confusion")
                .WithHero("Rogue").HasPowers("Diversion").At("Village")
                .Then(Verify.Player.Hero("Rogue").Commands.Includes("Diversion"))
                .When.Player.TakesAction("Diversion")
                .Then(Verify.Player.BlightSelectionView.Location("Village")
                    .WithBlights("Desecration", "Confusion"))
                .When.Player.SelectsBlight("Village", "Desecration")
                .Then(Verify.Player.BoardView.Location("Village").Blight("Desecration").IsSupressed())
                .Then(Verify.Player.Hero("Rogue").LostSecrecy(1));
        }

        [Test]
        public void Diversion_EndsWhenNecromancerEndsTurnAtLocation()
        {
            TestScenario.Game
                .Location("Village").HasBlights("Desecration")
                .WithHero("Rogue").HasPowers("Diversion").At("Village")
                .When.Player.TakesAction("Diversion")
                .When.Player.SelectsBlight("Village", "Desecration")
                .Given.Game.Necromancer.At("Village")
                .When.Game.NecromancerActs(Fake.Rolls(6)) // stays at village
                .When.Player.AcceptsNecromancerTurn()
                .Then(Verify.Player.BoardView.Location("Village").Blight("Desecration").IsSupressed(false));
        }

        [Test]
        public void Diversion_RequiresSecrecry()
        {
            TestScenario.Game
                .Location("Village").HasBlights("Desecration")
                .WithHero("Rogue").Secrecy(0).HasPowers("Diversion").At("Village")
                .Then(Verify.Player.Hero("Rogue").Commands.Excludes("Diversion"));
        }

        [Test]
        public void Diversion_RequiresTarget()
        {
            TestScenario.Game
                .Location("Village").HasBlights()
                .WithHero("Rogue").HasPowers("Diversion").At("Village")
                .Then(Verify.Player.Hero("Rogue").Commands.Excludes("Diversion"));
        }

        [Test]
        public void Diversion_Restored()
        {
            TestScenario.Game
                .Location("Village").HasBlights("Desecration")
                .WithHero("Rogue").HasPowers("Diversion").At("Village")
                .When.Player.TakesAction("Diversion")
                .When.Player.SelectsBlight("Village", "Desecration")
                .When.Game.Saved().Restored()
                .Then(Verify.Player.BoardView.Location("Village").Blight("Desecration").IsSupressed());
        }

        // Eavesdrop (Action): Spend 1 Secrecy to search with 2 dice.
        [Test]
        public void Eavesdrop()
        {
            TestScenario.Game
                .WithHero("Rogue").NotAt("Monastery").HasPowers("Eavesdrop")
                .Then(Verify.Player.Hero("Rogue").Commands.Includes("Eavesdrop"))
                .When.Player.TakesAction("Eavesdrop")
                .Then(Verify.Player.SearchView.Roll(6,6))
                .Then(Verify.Player.Hero("Rogue").LostSecrecy(1));
        }

        [Test]
        public void Eavesdrop_RequiresSecrecy()
        {
            TestScenario.Game
                .WithHero("Rogue").Secrecy(0).NotAt("Monastery").HasPowers("Eavesdrop")
                .Then(Verify.Player.Hero("Rogue").Commands.Excludes("Eavesdrop"));
        }

        [Test]
        public void Eavesdrop_NotAtMonastery()
        {
            TestScenario.Game
                .WithHero("Rogue").At("Monastery").HasPowers("Eavesdrop")
                .Then(Verify.Player.Hero("Rogue").Commands.Excludes("Eavesdrop"));
        }

        // Sabotage (Action): Spend 1 Secrecy in the Necromancer's location to cause -1 Darkness.
        [Test]
        public void Sabotage()
        {
            TestScenario.Game
                .Darkness(10)
                .Necromancer.At("Ruins")
                .WithHero("Rogue").At("Ruins").HasPowers("Sabotage")
                .WithHero() // add another hero to prevent necromancer's turn from starting
                .Given.Hero("Rogue").IsTakingTurn()
                .Then(Verify.Player.Hero("Rogue").Commands.Includes("Sabotage"))
                .When.Player.TakesAction("Rogue", "Sabotage")
                .Then(Verify.Player.BoardView.Darkness(9))
                .Then(Verify.Player.Hero("Rogue").LostSecrecy(1));
        }

        [Test]
        public void Sabotage_RequiresSecrecy()
        {
            TestScenario.Game
                .Necromancer.At("Ruins")
                .WithHero("Rogue").Secrecy(0).At("Ruins").HasPowers("Sabotage")
                .Then(Verify.Player.Hero("Rogue").Commands.Excludes("Sabotage"));
        }

        [Test]
        public void Sabotage_OnlyInNecromancerLocation()
        {
            TestScenario.Game
                .Necromancer.At("Ruins")
                .WithHero("Rogue").At("Village").HasPowers("Sabotage")
                .Then(Verify.Player.Hero("Rogue").Commands.Excludes("Sabotage"));
        }

        // Sap (Bonus): Exhaust during your turn to reduce the might of a blight in your location by 1 until your next turn.
        [Test]
        public void Sap()
        {
            TestScenario.Game
                .Location("Village").HasBlights("Desecration", "Confusion")
                .WithHero("Rogue").HasPowers("Sap").At("Village")
                .Then(Verify.Player.Hero("Rogue").Commands.Includes("Sap"))
                .When.Player.TakesAction("Sap")
                .Then(Verify.Player.BlightSelectionView.Location("Village")
                    .WithBlights("Desecration", "Confusion"))
                .When.Player.SelectsBlight("Village", "Desecration")
                .Then(Verify.Player.BoardView.Location("Village").Blight("Desecration").Might(3))
                .Then(Verify.Power("Sap").IsExhausted())
                .Given.Hero("Rogue").IsTakingTurn(false)
                .When.Player.TakesAction("Rogue", "Start Turn")
                .Then(Verify.Player.BoardView.Location("Village").Blight("Desecration").Might(4));
        }

        [Test]
        public void Sap_MustBeBlightAtHeroLocation()
        {
            TestScenario.Game
                .Location("Village").HasBlights()
                .WithHero("Rogue").HasPowers("Sap").At("Village")
                .Then(Verify.Player.Hero("Rogue").Commands.Excludes("Sap"));
        }

        [Test]
        public void Sap_MustBeUsedDuringTurn()
        {
            TestScenario.Game
                .Location("Village").HasBlights("Desecration")
                .WithHero("Rogue").HasPowers("Sap").At("Village").IsTakingTurn(false)
                .Then(Verify.Player.Hero("Rogue").Commands.Excludes("Sap"));
        }

        // Shadow Cloak (Bonus): +1 die when eluding.
        [Test]
        public void ShadowCloak()
        {
            TestScenario.Game
                .WithHero("Rogue").HasPowers("Shadow Cloak")
                .Then(Verify.Hero("Rogue").EludeDice(2));
        }

        // Skulk (Tactic): Elude with 2 dice and add 1 to the highest die.
        [Test]
        public void Skulk()
        {
            TestScenario.Game
                .WithHero("Rogue").HasPowers("Skulk").IsFacingEnemy("Skeleton")
                .Then(Verify.Player.ConflictModel.HasTactics("Skulk", "Fight", "Elude"))
                .When.Player.Targets("Skeleton").UsesTactic("Skulk").ResolvesConflict(Fake.Rolls(2, 2))
                .Then(Verify.Player.ConflictModel.Rolled(3, 2));
        }

        // Stealth (Bonus): Any time you lose or spend Secrecy, you can spend 1 Grace instead.
        [Test]
        public void Stealth()
        {
            TestScenario.Game
                .WithHero("Rogue").HasPowers("Stealth", "Eavesdrop").NotAt("Monastery")
                .When.Player.TakesAction("Eavesdrop")
                .Then(Verify.Player.Question("Spend 1 Grace instead of Secrecy?", "Yes", "No"))
                .When.Player.AnswersQuestion("Stealth", "Yes")
                .Then(Verify.Player.SearchView.Roll(6, 6))
                .Then(Verify.Player.Hero("Rogue").LostSecrecy(0).LostGrace(1));
        }

        [Test]
        public void Stealth_CanSpendSecrecy()
        {
            TestScenario.Game
                .WithHero("Rogue").Secrecy(0).HasPowers("Stealth")
                .Then(Verify.Hero("Rogue").CanSpendSecrecy());
        }

        // Vanish (Tactic): Elude with 2 dice. Gain 1 Secrecy (up to 7) if you roll 2 successes.
        [Test]
        public void Vanish()
        {
            TestScenario.Game
                .WithHero("Rogue").Secrecy(0).HasPowers("Vanish").IsFacingEnemy("Zombie")
                .Then(Verify.Player.ConflictModel.HasTactics("Vanish", "Fight", "Elude"))
                .When.Player.Targets("Zombie").UsesTactic("Vanish").ResolvesConflict(Fake.Rolls(3, 3))
                .Then(Verify.Player.ConflictModel.Rolled(3, 3))
                .When.Player.AcceptsRoll()
                .Then(Verify.Player.Hero("Rogue").Secrecy(1));
        }

        [Test]
        public void Vanish_NoEffectWithLessThanTwoSuccesses()
        {
            TestScenario.Game
                .WithHero("Rogue").Secrecy(0).HasPowers("Vanish").IsFacingEnemy("Zombie")
                .Then(Verify.Player.ConflictModel.HasTactics("Vanish", "Fight", "Elude"))
                .When.Player.Targets("Zombie").UsesTactic("Vanish").ResolvesConflict(Fake.Rolls(2, 3))
                .When.Player.AcceptsRoll()
                .Then(Verify.Player.Hero("Rogue").Secrecy(0));
        }

        [Test]
        public void Vanish_MaximumOf7()
        {
            TestScenario.Game
                .WithHero("Rogue").Secrecy(7).HasPowers("Vanish").IsFacingEnemy("Zombie")
                .Then(Verify.Player.ConflictModel.HasTactics("Vanish", "Fight", "Elude"))
                .When.Player.Targets("Zombie").UsesTactic("Vanish").ResolvesConflict(Fake.Rolls(3, 3))
                .When.Player.AcceptsRoll()
                .Then(Verify.Player.Hero("Rogue").Secrecy(7));
        }
    }
}

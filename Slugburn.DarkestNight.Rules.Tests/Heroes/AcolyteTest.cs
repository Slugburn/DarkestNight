using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class AcolyteTest
    {
        // Fade to Black (Bonus): +1 die in fights when Darkness is 10 or more. Another +1 die in fights when Darkness is 20 or more.
        [TestCase(9, 0)]
        [TestCase(10, 1)]
        [TestCase(19, 1)]
        [TestCase(20, 2)]
        public void FadeToBlack(int darkness, int bonusDice)
        {
            var expectedDice = 1 + bonusDice;
            TestScenario
                .Game.WithHero("Acolyte").HasPowers("Fade to Black")
                .Game.Darkness(darkness)
                .Then(Verify.Hero().FightDice(expectedDice));
        }

        // False Life (Bonus): Exhaust at any time while not at the Monastery to gain 1 Grace (up to default). You may not enter the Monastery while this power is exhausted.
        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        [TestCase(4, false)]
        public void FalseLife_NotUsableWhenAtOrAboveDefaultGrace(int grace, bool isAvailable)
        {
            TestScenario
                .Game.WithHero("Acolyte").HasPowers("False Life").Grace(grace).NotAt("Monastery")
                .Then(Verify.Hero().Grace(grace).CanTakeAction("False Life", isAvailable));
        }

        // Final Rest (Tactic): Fight with 2d or 3d. If any die comes up a 1, lose 1 Grace.
        [TestCase(2, new[] {1, 5})]
        [TestCase(3, new[] {1, 1, 6})]
        public void FinalRest(int count, int[] rolls)
        {
            var tacticName = $"Final Rest [{count}d]";
            TestScenario
                .Game.WithHero("Acolyte").HasPowers("Final Rest").Grace(3)
                .Given.Hero().IsFacingEnemy("Skeleton")
                .Then(Verify.Player.ConflictView.HasTactics("Fight", "Elude", "Final Rest [2d]", "Final Rest [3d]"))
                .When.Player.CompletesConflict("Skeleton", tacticName, Fake.Rolls(rolls))
                .Then(Verify.Hero().LostGrace());
        }

        // Blinding Black (Bonus): Exhaust after a Necromancer movement roll to prevent him from detecting any heroes, regardless of Secrecy.
        [Test]
        public void BlindingBlack()
        {
            TestScenario.Game
                .WithHero("Acolyte").HasPowers("Blinding Black").At("Swamp").Secrecy(0)
                .Necromancer.At("Castle")
                .When.Game.NecromancerActs(Fake.Rolls(5))
                .Then(Verify.Hero().CanTakeAction("Blinding Black"))
                .Then(Verify.Player.NecromancerView.Roll(5).Detected("Acolyte").MovingTo("Swamp"))
                .When.Player.TakesAction("Blinding Black")
                .Then(Verify.Power("Blinding Black").IsExhausted())
                .Then(Verify.Player.NecromancerView.Roll(5).Detected().MovingTo("Village"))
                .When.Player.AcceptsNecromancerTurn()
                .Then(Verify.Game.Darkness(1).Necromancer.At("Village"));
        }

        // Call to Death (Action): Attack two blights in your location at once. Make a single fight roll with +1 die, 
        // then divide the dice between blights and resolve as two separate attacks (losing Secrecy for each).
        [Test]
        public void CallToDeath()
        {
            TestScenario.Game
                .WithHero("Acolyte").HasPowers("Call to Death").At("Swamp")
                .Given.Location("Swamp").HasBlights("Skeletons", "Shades", "Lich")
                .When.Player.TakesAction("Call to Death")
                .Then(Verify.Player.ConflictView.HasTargets("Skeletons", "Shades", "Lich").HasTactics("Fight").MustSelectTargets(2))
                .When.Player.Targets("Skeletons", "Lich").UsesTactic("Fight").ResolvesConflict(Fake.Rolls(5,6)).AcceptsRoll()
                .Then(Verify.Player.ConflictView.Rolled(5, 6))
                .When.Player.AssignsDie(6, "Lich").AssignsDie(5, "Skeletons").AcceptsConflictResults(2)
                .Then(Verify.Location("Swamp").Blights("Shades"))
                .Then(Verify.Hero().LostSecrecy(2).HasUsedAction());
        }

        [Test]
        public void CallToDeath_CombinedWith_FinalRest()
        {
            TestScenario.Game
                .WithHero("Acolyte").HasPowers("Call to Death", "Final Rest").At("Swamp")
                .Given.Location("Swamp").HasBlights("Skeletons", "Shades")
                .When.Player.TakesAction("Call to Death")
                .When.Player.Targets("Skeletons", "Shades").UsesTactic("Final Rest [3d]").ResolvesConflict(Fake.Rolls(5, 2, 3, 1)).AcceptsRoll()
                .When.Player.AssignsDie(5, "Shades").AssignsDie(3, "Skeletons").AcceptsConflictResults(2)
                .Then(Verify.Location("Swamp").Blights("Skeletons"))
                .Then(Verify.Hero()
                    .WasWounded()
                    .LostGrace(2) // loses Grace from failing to kill Skeletons and rolling a 1 with Final Rest
                    .LostSecrecy(2) // loses 2 Secrecy from making 2 attacks
                    .HasUsedAction());
        }

        // Dark Veil (Bonus): Exhaust at any time to ignore blights' effects until your next turn. 
        // *OR* Exhaust after you fail an attack on a blight to ignore its Defense.
        [Test]
        public void DarkVeil_IgnoreBlightDefense()
        {
            TestScenario.Game
                .WithHero("Acolyte").HasPowers("Dark Veil").At("Swamp")
                .Location("Swamp").HasBlights("Spies")
                .When.Player.TakesAction("Attack").Targets("Spies").ResolvesConflict(Fake.Rolls(1)).AcceptsRoll()
                .When.Player.TakesAction("Dark Veil [ignore defense]")
                .Then(Verify.Power("Dark Veil").IsExhausted())
                .When.Player.AcceptsConflictResults()
                .Then(Verify.Hero().HasUsedAction().LostSecrecy()); // loses Secrecy for making attack, but not for Spies defense
        }

        [Test]
        public void DarkVeil_IgnoreBlightEffects()
        {
            TestScenario
                .Game.WithHero("Acolyte").HasPowers("Dark Veil")
                .When.Player.TakesAction("Dark Veil [ignore effects]")
                .Then(Verify.Hero().IsIgnoringBlights())
                .Then(Verify.Power("Dark Veil").IsExhausted())
                .When.Player.StartsTurn()
                .Then(Verify.Hero().IsNotIgnoringBlights());
        }

        // Death Mask (Bonus): You may choose not to lose Secrecy for attacking a blight (including use of the Call to Death power) 
        // or for starting your turn at the Necromancer's location.
        [Test]
        public void DeathMask_IgnoreSecrecyLossForAttacking()
        {
            TestScenario.Game
                .WithHero("Acolyte").HasPowers("Death Mask").At("Swamp")
                .Location("Swamp").HasBlights("Spies")
                .When.Player.TakesAction("Attack").Fights(Fake.Rolls(1))
                .Then(Verify.Hero().HasUsedAction().LostSecrecy()); // loses Secrecy for Spies defense, but not for making attack
        }

        [Test]
        public void DeathMask_IgnoreSecrecyLossForBeingInNecromancersLocation()
        {
            TestScenario.Game
                .Necromancer.At("Swamp")
                .WithHero("Acolyte").HasPowers("Death Mask").At("Swamp")
                .When.Player.StartsTurn()
                .Then(Verify.Hero().LostSecrecy(0).HasUnresolvedEvents(1));
        }

        [Test]
        public void FadeToBlack_CombinedWith_FinalRest()
        {
            var rolls = Enumerable.Repeat(6, 5).ToArray();
            TestScenario.Game
                .Darkness(20)
                .WithHero("Acolyte").HasPowers("Fade to Black", "Final Rest")
                .Given.Hero().IsFacingEnemy("Skeleton")
                .When.Player.CompletesConflict("Skeleton", "Final Rest [3d]", Fake.Rolls(rolls))
                .Then(Verify.Hero().FightDice(3).RolledNumberOfDice(5));
        }

        [Test]
        public void FalseLife()
        {
            TestScenario
                .Game.WithHero("Acolyte").HasPowers("False Life").At("Swamp").Grace(0)
                .When.Player.TakesAction("False Life")
                .Then(Verify.Hero().Grace(1))
                .Then(Verify.Power("False Life").IsExhausted());
        }

        [Test]
        public void FalseLife_NotUsableInMonastery()
        {
            TestScenario.Game
                .WithHero("Acolyte").HasPowers("False Life").At("Monastery").Grace(0)
                .Then(Verify.Hero().Grace(0).CanTakeAction("False Life", false));
        }

        [Test]
        public void FalseLife_PreventsMovementToMonasteryWhileExhausted()
        {
            TestScenario
                .Game.WithHero("Acolyte").HasPowers("False Life").At("Village").Grace(2)
                .Then(Verify.Hero().Grace(2).CanMoveTo("Monastery"))
                .When.Player.TakesAction("False Life")
                .Then(Verify.Hero().CannotMoveTo("Monastery"))
                .Given.Hero().RefreshesPower("False Life")
                .Then(Verify.Hero().CanMoveTo("Monastery"));
        }

        [Test]
        public void FalseOrders()
        {
            TestScenario.Game
                .WithHero("Acolyte").HasPowers("False Orders").At("Village")
                .Location("Village").HasBlights("Confusion", "Corruption", "Shroud", "Skeletons")
                .Location("Monastery").HasBlights("Lich")
                .When.Player.TakesAction("False Orders")
                .Then(Verify.Player.LocationSelectionView("Monastery", "Mountains", "Swamp", "Castle", "Ruins", "Forest"))
                .When.Player.SelectsLocation("Monastery")
                .Then(Verify.Player.BlightSelectionView.Max(3).Location("Village").WithBlights("Confusion", "Corruption", "Shroud", "Skeletons"))
                .When.Player.SelectsBlights("Confusion", "Corruption", "Shroud")
                .Then(Verify.Hero().HasUsedAction())
                .Then(Verify.Location("Village").Blights("Skeletons"))
                .Then(Verify.Location("Monastery").Blights("Lich", "Confusion", "Corruption", "Shroud"));
        }

        // Forbidden Arts (Bonus): After a fight roll, add any number of dice, one at a time. For each added die that comes up a 1, +1 Darkness.
        [Test]
        public void ForbiddenArts()
        {
            TestScenario.Game
                .Darkness(0)
                .WithHero("Acolyte").HasPowers("Forbidden Arts")
                .Given.Hero().IsFacingEnemy("Skeleton")
                .When.Player.Targets("Skeleton").UsesTactic("Fight").ResolvesConflict(Fake.Rolls(1))
                .When.Player.TakesAction("Forbidden Arts", Fake.Rolls(1))
                .Then(Verify.Game.Darkness(1)) // rolling a 1 increases the Darkness
                .Then(Verify.Player.ConflictView.Rolled(1, 1))
                .When.Player.TakesAction("Forbidden Arts", Fake.Rolls(4))
                .Then(Verify.Player.ConflictView.Rolled(1, 1, 4))
                .When.Player.AcceptsRoll().AcceptsConflictResults()
                .Then(Verify.Hero().WasWounded(false));
        }

        // Leech Life (Tactic): Exhaust while not at the Monastery to fight with 3 dice. Gain 1 Grace (up to default) if you roll 2 successes. 
        // You may not enter the Monastery while this power is exhausted.
        [Test]
        public void LeechLife()
        {
            TestScenario.Game
                .WithHero("Acolyte").HasPowers("Leech Life").Grace(1).NotAt("Monastery")
                .Given.Hero().IsFacingEnemy("Skeleton")
                .When.Player.CompletesConflict("Skeleton", "Leech Life", Fake.Rolls(1, 5, 6))
                .Then(Verify.Hero().Grace(2))
                .Then(Verify.Power("Leech Life").IsExhausted());
        }

        [Test]
        public void LeechLife_NotUsableInMonastery()
        {
            TestScenario.Game
                .WithHero("Acolyte").HasPowers("Leech Life").At("Monastery")
                .Then(Verify.Hero().CanTakeAction("Leech Life", false));
        }

        [Test]
        public void LeechLife_PreventsMovementToMonasteryWhileExhausted()
        {
            TestScenario.Game
                .WithHero("Acolyte").HasPowers("Leech Life").At("Village")
                .Power("Leech Life").IsExhausted()
                .Then(Verify.Hero().CannotMoveTo("Monastery"));
        }
    }
}
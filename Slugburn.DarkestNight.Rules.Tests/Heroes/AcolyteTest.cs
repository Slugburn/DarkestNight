using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

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
                .Given.Game.WithHero("Acolyte").HasPowers("Fade to Black")
                .Given.Game.Darkness(darkness)
                .Then(Verify.Hero.FightDice(expectedDice));
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
                .Given.Game.WithHero("Acolyte").HasPowers("False Life").NotAt("Monastery").Grace(grace)
                .Then(Verify.Hero.Grace(grace).CanTakeAction("False Life", isAvailable));
        }

        // Final Rest (Tactic): Fight with 2d or 3d. If any die comes up a 1, lose 1 Grace.
        [TestCase(2, new[] {1, 5})]
        [TestCase(3, new[] {1, 1, 6})]
        public void FinalRest(int count, int[] rolls)
        {
            var tacticName = $"Final Rest [{count}d]";
            TestScenario
                .Given.Game.WithHero("Acolyte").HasPowers("Final Rest").Grace(3)
                .When.Hero.FacesEnemy("Skeleton")
                .Then(Verify.Player.ConflictView.HasTactics("Fight", "Elude", "Final Rest [2d]", "Final Rest [3d]"))
                .When.Player.ResolvesConflict(x => x.Target("Skeleton").Tactic(tacticName).Rolls(rolls)).AcceptsRoll()
                .Then(Verify.Hero.Grace(2));
        }

        // Blinding Black (Bonus): Exhaust after a Necromancer movement roll to prevent him from detecting any heroes, regardless of Secrecy.
        [Test]
        public void BlindingBlack()
        {
            TestScenario.Given.Game
                .WithHero("Acolyte").HasPowers("Blinding Black").At("Swamp").Secrecy(0)
                .NecromancerAt("Castle")
                .When.Game.NecromancerActs(Fake.Rolls(5))
                .Then(Verify.Hero.CanTakeAction("Blinding Black"))
                .Then(Verify.Player.NecromancerView.Roll(5).Detected("Acolyte").MovingTo("Swamp"))
                .When.Player.TakesAction("Blinding Black")
                .Then(Verify.Power("Blinding Black").IsExhausted())
                .Then(Verify.Player.NecromancerView.Roll(5).Detected().MovingTo("Village"))
                .When.Player.FinishNecromancerTurn()
                .Then(Verify.Game.NecromancerAt("Village").Darkness(1));
        }

        // Call to Death (Action): Attack two blights in your location at once. Make a single fight roll with +1 die, 
        // then divide the dice between blights and resolve as two separate attacks (losing Secrecy for each).
        [Test]
        public void CallToDeath()
        {
            TestScenario.Given.Game
                .WithHero("Acolyte").HasPowers("Call to Death").At("Swamp")
                .Given.Location("Swamp").Blights("Skeletons", "Shades", "Lich")
                .When.Player.TakesAction("Call to Death")
                .Then(Verify.Player.ConflictView.HasTargets("Skeletons", "Shades", "Lich").HasTactics("Fight").MustSelectTargets(2))
                .When.Player.ResolvesConflict(x => x.Tactic("Fight").Target("Skeletons", "Lich").Rolls(5, 6)).AcceptsRoll()
                .Then(Verify.Player.ConflictView.Rolled(5, 6))
                .When.Player.AssignsDie(6, "Lich").AssignsDie(5, "Skeletons").AcceptsConflictResults()
                .Then(Verify.Location("Swamp").Blights("Shades"))
                .Then(Verify.Hero.LostSecrecy(2).HasUsedAction());
        }

        [Test]
        public void CallToDeath_CombinedWith_FinalRest()
        {
            TestScenario.Given.Game
                .WithHero("Acolyte").HasPowers("Call to Death", "Final Rest").At("Swamp")
                .Given.Location("Swamp").Blights("Skeletons", "Shades")
                .When.Player.TakesAction("Call to Death")
                .When.Player.ResolvesConflict(x => x.Tactic("Final Rest [3d]").Target("Skeletons", "Shades").Rolls(5, 2, 3, 1)).AcceptsRoll()
                .When.Player.AssignsDie(5, "Shades").AssignsDie(3, "Skeletons").AcceptsConflictResults()
                .Then(Verify.Location("Swamp").Blights("Skeletons"))
                .Then(Verify.Hero
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
            TestScenario.Given.Game
                .WithHero("Acolyte").HasPowers("Dark Veil").At("Swamp")
                .Location("Swamp").Blights("Spies")
                .When.Player.TakesAction("Attack").ResolvesConflict(x => x.Tactic("Fight").Target("Spies").Rolls(1)).AcceptsRoll()
                .When.Player.TakesAction("Dark Veil [ignore defense]")
                .Then(Verify.Power("Dark Veil").IsExhausted())
                .When.Player.AcceptsConflictResults()
                .Then(Verify.Hero.HasUsedAction().LostSecrecy()); // loses Secrecy for making attack, but not for Spies defense
        }

        [Test]
        public void DarkVeil_IgnoreBlightEffects()
        {
            TestScenario
                .Given.Game.WithHero("Acolyte").HasPowers("Dark Veil")
                .When.Player.TakesAction("Dark Veil [ignore effects]")
                .Then(Verify.Hero.IsIgnoringBlights())
                .Then(Verify.Power("Dark Veil").IsExhausted())
                .When.Hero.StartsTurn()
                .Then(Verify.Hero.IsNotIgnoringBlights());
        }

        // Death Mask (Bonus): You may choose not to lose Secrecy for attacking a blight (including use of the Call to Death power) 
        // or for starting your turn at the Necromancer's location.
        [Test]
        public void DeathMask_IgnoreSecrecyLossForAttacking()
        {
            TestScenario.Given.Game
                .WithHero("Acolyte").HasPowers("Death Mask").At("Swamp")
                .Location("Swamp").Blights("Spies")
                .When.Player.TakesAction("Attack").ResolvesConflict(x => x.Tactic("Fight").Target("Spies").Rolls(1)).AcceptsRoll().AcceptsConflictResults()
                .Then(Verify.Hero.HasUsedAction().LostSecrecy()); // loses Secrecy for Spies defense, but not for making attack
        }

        [Test]
        public void DeathMask_IgnoreSecrecyLossForBeingInNecromancersLocation()
        {
            TestScenario
                .Given.Game.NecromancerAt("Swamp")
                .WithHero("Acolyte").HasPowers("Death Mask").At("Swamp")
                .When.Hero.StartsTurn()
                .Then(Verify.Hero.LostSecrecy(0));
        }

        [Test]
        public void FadeToBlack_CombinedWith_FinalRest()
        {
            var rolls = Enumerable.Repeat(6, 5).ToArray();
            new TestScenario()
                .GivenDarkness(20)
                .GivenHero("Acolyte", x => x.HasPowers("Fade to Black", "Final Rest").At("Monastery"))
                .GivenLocation("Monastery", x => x.Blights("Skeletons"))
                .WhenPlayerTakesAttackAction(x => x.Tactic("Final Rest [3d]").Rolls(rolls))
                .ThenHero(x => x.FightDice(3).RolledNumberOfDice(5).HasUsedAction().LostSecrecy());
        }

        [Test]
        public void FalseLife()
        {
            TestScenario
                .Given.Game.WithHero("Acolyte").HasPowers("False Life").At("Swamp").Grace(0)
                .When.Player.TakesAction("False Life")
                .Then(Verify.Hero.Grace(1))
                .Then(Verify.Power("False Life").IsExhausted());
        }

        [Test]
        public void FalseLife_NotUsableInMonastery()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("False Life").At("Monastery").Grace(0))
                .ThenHero(x => x.Grace(0).CanUsePower("False Life", false));
        }

        [Test]
        public void FalseLife_PreventsMovementToMonasteryWhileExhausted()
        {
            TestScenario
                .Given.Game.WithHero("Acolyte").HasPowers("False Life").At("Village").Grace(2)
                .Then(Verify.Hero.Grace(2).CanMoveTo("Monastery"))
                .When.Player.TakesAction("False Life")
                .Then(Verify.Hero.CannotMoveTo("Monastery"))
                .When.Hero.RefreshesPower("False Life")
                .Then(Verify.Hero.CanMoveTo("Monastery"));
        }

        [Test]
        public void FalseOrders()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("False Orders").At("Village"))
                .GivenLocation("Village", x => x.Blights("Confusion", "Corruption", "Shroud", "Skeletons"))
                .GivenLocation("Monastery", x => x.Blights("Lich"))
                .WhenPlayerTakesAction("False Orders",
                    x => x.ChooseLocation(Location.Monastery)
                        .ChoosesBlight("Confusion", "Corruption", "Shroud"))
                .ThenHero(x => x.HasUsedAction())
                .ThenSpace("Village", x => x.Blights("Skeletons"))
                .ThenSpace("Monastery", x => x.Blights("Lich", "Confusion", "Corruption", "Shroud"));
        }

        [Test]
        public void FalseOrders_CancelChooseBlights()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("False Orders").At("Village"))
                .GivenLocation("Village", x => x.Blights("Confusion", "Corruption", "Shroud", "Skeletons"))
                .GivenLocation("Monastery", x => x.Blights("Lich"))
                .WhenPlayerTakesAction("False Orders",
                    x => x.ChooseLocation(Location.Monastery).ChoosesBlight("None"))
                .ThenHero(x => x.HasNotUsedAction());
        }

        [Test]
        public void FalseOrders_CancelChooseLocation()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("False Orders").At("Village"))
                .GivenLocation("Village", x => x.Blights("Confusion", "Corruption", "Shroud", "Skeletons"))
                .GivenLocation("Monastery", x => x.Blights("Lich"))
                .WhenPlayerTakesAction("False Orders", x => x.ChooseLocation(Location.None))
                .ThenHero(x => x.HasNotUsedAction());
        }

        [Test]
        public void ForbiddenArts()
        {
            new TestScenario()
                .GivenDarkness(0)
                .GivenHero("Acolyte", x => x.HasPowers("Forbidden Arts").At("Village"))
                .GivenLocation("Village", x => x.Blights("Confusion"))
                .WhenPlayerTakesAction("Attack")
                .WhenPlayerSelectsTactic(x => x.Rolls(1))
                .WhenPlayerTakesAction("Forbidden Arts", x => x.Rolls(1))
                .ThenDarkness(1)
                .WhenPlayerTakesAction("Forbidden Arts", x => x.Rolls(4))
                .WhenPlayerAcceptsRoll()
                .ThenHero(x => x.HasUsedAction().Secrecy(6));
        }

        [Test]
        public void LeechLife()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("Leech Life").At("Village").Grace(1))
                .GivenLocation("Village", x => x.Blights("Corruption"))
                .WhenPlayerTakesAttackAction(x => x.Tactic("Leech Life").Rolls(1, 5, 6))
                .ThenSpace("Village", x => x.NoBlights())
                .ThenHero(x => x.HasUsedAction().Grace(2).LostSecrecy()) // two successes gains a Grace
                .ThenPower("Leech Life", x => x.IsExhausted());
        }

        [Test]
        public void LeechLife_NotUsableInMonastery()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("Leech Life").At("Monastery"))
                .ThenHero(x => x.CanUsePower("Leech Life", false));
        }

        [Test]
        public void LeechLife_PreventsMovementToMonasteryWhileExhausted()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("Leech Life").At("Village"))
                .GivenPower("Leech Life", x => x.IsExhausted())
                .ThenHero(x => x.CannotMoveTo("Monastery"));
        }
    }
}
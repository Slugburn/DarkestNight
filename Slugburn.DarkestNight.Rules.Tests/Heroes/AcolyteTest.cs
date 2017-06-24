using System;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class AcolyteTest
    {
        [TestCase(9, 0)]
        [TestCase(10, 1)]
        [TestCase(19, 1)]
        [TestCase(20, 2)]
        public void FadeToBlack(int darkness, int bonusDice)
        {
            var expectedDice = 1 + bonusDice;
            var rolls = Enumerable.Repeat(6, expectedDice).ToArray();
            new TestScenario()
                .GivenDarkness(darkness)
                .GivenHero("Acolyte", x => x.HasPowers("Fade to Black").Location("Monastery"))
                .GivenLocation("Monastery", x => x.Blight("Skeletons"))
                .WhenPlayerTakesAttackAction(x => x.Rolls(rolls))
                .ThenHero(x => x.FightDice(expectedDice).RolledNumberOfDice(expectedDice).HasUsedAction().LostSecrecy());
        }

        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        [TestCase(4, false)]
        public void FalseLife_NotUsableWhenAtOrAboveDefaultGrace(int grace, bool usable)
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("False Life").Location("Swamp").Grace(grace))
                .ThenHero(x => x.Grace(grace).CanUsePower("False Life", usable));
        }

        [TestCase(2, new[] {1, 5})]
        [TestCase(3, new[] {1, 1, 6})]
        public void FinalRest(int count, int[] rolls)
        {
            var tacticName = $"Final Rest ({count} dice)";
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("Final Rest").Location("Village").Grace(3).Secrecy(7))
                .GivenLocation("Village", x => x.Blight("Corruption"))
                .WhenPlayerTakesAttackAction(x => x.Tactic(tacticName).Rolls(rolls))
                .ThenSpace("Village", x => x.NoBlights())
                .ThenHero(x => x.HasUsedAction().Grace(2).Secrecy(6));
        }

        [Test]
        public void BlindingBlack()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("Blinding Black").Location("Swamp").Secrecy(0))
                .GivenNecromancerLocation("Castle")
                .WhenNecromancerTakesTurn(x => x.Rolls(5), player => player.UsePower("Blinding Black"))
                .ThenNecromancerLocation("Village")
                .ThenPower("Blinding Black", x => x.IsExhausted());
        }

        [Test]
        public void CallToDeath()
        {
            new TestScenario()
                .GivenLocation("Swamp", x => x.Blight("Skeletons", "Shades", "Lich"))
                .GivenHero("Acolyte", x => x.HasPowers("Call to Death").Location("Swamp"))
                .WhenPlayerTakesAttackAction(x => x.Action("Call to Death").Target("Skeletons", "Lich").Rolls(5, 6))
                .WhenPlayerAssignsRolledDiceToBlights(Tuple.Create("Lich", 6), Tuple.Create("Skeletons", 5))
                .ThenSpace("Swamp", x => x.Blights("Shades"))
                .ThenHero(x => x.LostSecrecy(2).HasUsedAction());
        }

        [Test]
        public void CallToDeath_CombinedWith_FinalRest()
        {
            new TestScenario()
                .GivenLocation("Swamp", x => x.Blight("Skeletons", "Shades"))
                .GivenHero("Acolyte", x => x.HasPowers("Call to Death", "Final Rest").Location("Swamp"))
                .WhenPlayerTakesAttackAction(x => x.Action("Call to Death").Tactic("Final Rest (3 dice)").Target("Skeletons", "Shades").Rolls(5, 2, 3, 1))
                .WhenPlayerAssignsRolledDiceToBlights(Tuple.Create("Shades", 5), Tuple.Create("Skeletons", 3))
                .ThenSpace("Swamp", x => x.Blights("Skeletons"))
                .ThenHero(x => x.HasUsedAction()
                    .WasWounded()
                    .LostGrace(2) // loses Grace from failing to kill Skeletons and rolling a 1 with Final Rest
                    .LostSecrecy(2)); // loses 2 Secrecy from making 2 attacks
        }

        [Test]
        public void DarkVeil_IgnoreBlightDefense()
        {
            new TestScenario()
                .GivenLocation("Swamp", x => x.Blight("Spies"))
                .GivenHero("Acolyte", x => x.HasPowers("Dark Veil").Location("Swamp"))
                .WhenPlayerTakesAttackAction(player => player.UsePower("Dark Veil").Rolls(1))
                .ThenHero(x => x.HasUsedAction().LostSecrecy()) // loses Secrecy for making attack, but not for Spies defense
                .ThenPower("Dark Veil", x => x.IsExhausted());
        }

        [Test]
        public void DarkVeil_IgnoreBlightEffects()
        {
            TestScenario
                .Given.Game.WithHero("Acolyte", x => x.HasPowers("Dark Veil"))
                .When.Player.TakesAction("Dark Veil")
                .Then(Verify.Hero.IsIgnoringBlights())
                .Then(Verify.Power("Dark Veil").IsExhausted())
                .When.Hero.StartsTurn()
                .Then(Verify.Hero.IsNotIgnoringBlights());
        }

        [Test]
        public void DeathMask_IgnoreSecrecyLossForAttacking()
        {
            new TestScenario()
                .GivenLocation("Swamp", x => x.Blight("Spies"))
                .GivenHero("Acolyte", x => x.HasPowers("Death Mask").Location("Swamp"))
                .WhenPlayerTakesAttackAction(player => player.Rolls(1))
                .ThenHero(x => x.HasUsedAction().LostSecrecy()); // loses Secrecy for Spies defense, but not for making attack
        }

        [Test]
        public void DeathMask_IgnoreSecrecyLossForBeingInNecromancersLocation()
        {
            TestScenario
                .Given.Game.NecromancerIn("Swamp")
                .WithHero("Acolyte", x => x.HasPowers("Death Mask").Location("Swamp"))
                .When.Hero.StartsTurn()
                .Then(Verify.Hero.LostSecrecy(0));
        }

        [Test]
        public void FadeToBlack_CombinedWith_FinalRest()
        {
            var rolls = Enumerable.Repeat(6, 5).ToArray();
            new TestScenario()
                .GivenDarkness(20)
                .GivenHero("Acolyte", x => x.HasPowers("Fade to Black", "Final Rest").Location("Monastery"))
                .GivenLocation("Monastery", x => x.Blight("Skeletons"))
                .WhenPlayerTakesAttackAction(x => x.Tactic("Final Rest (3 dice)").Rolls(rolls))
                .ThenHero(x => x.FightDice(3).RolledNumberOfDice(5).HasUsedAction().LostSecrecy());
        }

        [Test]
        public void FalseLife()
        {
            TestScenario
                .Given.Game.WithHero("Acolyte", x => x.HasPowers("False Life").Location("Swamp").Grace(0))
                .When.Player.TakesAction("False Life")
                .Then(Verify.Hero.Grace(1))
                .Then(Verify.Power("False Life").IsExhausted());
        }

        [Test]
        public void FalseLife_NotUsableInMonastery()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("False Life").Location("Monastery").Grace(0))
                .ThenHero(x => x.Grace(0).CanUsePower("False Life", false));
        }

        [Test]
        public void FalseLife_PreventsMovementToMonasteryWhileExhausted()
        {
            TestScenario
                .Given.Game.WithHero("Acolyte", x => x.HasPowers("False Life").Location("Village").Grace(2))
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
                .GivenHero("Acolyte", x => x.HasPowers("False Orders").Location("Village"))
                .GivenLocation("Village", x => x.Blight("Confusion", "Corruption", "Shroud", "Skeletons"))
                .GivenLocation("Monastery", x => x.Blight("Lich"))
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
                .GivenHero("Acolyte", x => x.HasPowers("False Orders").Location("Village"))
                .GivenLocation("Village", x => x.Blight("Confusion", "Corruption", "Shroud", "Skeletons"))
                .GivenLocation("Monastery", x => x.Blight("Lich"))
                .WhenPlayerTakesAction("False Orders",
                    x => x.ChooseLocation(Location.Monastery).ChoosesBlight("None"))
                .ThenHero(x => x.HasNotUsedAction());
        }

        [Test]
        public void FalseOrders_CancelChooseLocation()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("False Orders").Location("Village"))
                .GivenLocation("Village", x => x.Blight("Confusion", "Corruption", "Shroud", "Skeletons"))
                .GivenLocation("Monastery", x => x.Blight("Lich"))
                .WhenPlayerTakesAction("False Orders", x => x.ChooseLocation(Location.None))
                .ThenHero(x => x.HasNotUsedAction());
        }

        [Test]
        public void ForbiddenArts()
        {
            new TestScenario()
                .GivenDarkness(0)
                .GivenHero("Acolyte", x => x.HasPowers("Forbidden Arts").Location("Village"))
                .GivenLocation("Village", x => x.Blight("Confusion"))
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
                .GivenHero("Acolyte", x => x.HasPowers("Leech Life").Location("Village").Grace(1))
                .GivenLocation("Village", x => x.Blight("Corruption"))
                .WhenPlayerTakesAttackAction(x => x.Tactic("Leech Life").Rolls(1, 5, 6))
                .ThenSpace("Village", x => x.NoBlights())
                .ThenHero(x => x.HasUsedAction().Grace(2).LostSecrecy()) // two successes gains a Grace
                .ThenPower("Leech Life", x => x.IsExhausted());
        }

        [Test]
        public void LeechLife_NotUsableInMonastery()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("Leech Life").Location("Monastery"))
                .ThenHero(x => x.CanUsePower("Leech Life", false));
        }

        [Test]
        public void LeechLife_PreventsMovementToMonasteryWhileExhausted()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("Leech Life").Location("Village"))
                .GivenPower("Leech Life", x => x.IsExhausted())
                .ThenHero(x => x.CannotMoveTo("Monastery"));
        }
    }
}
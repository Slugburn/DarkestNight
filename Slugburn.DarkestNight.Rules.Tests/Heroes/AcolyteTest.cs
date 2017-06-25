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

        [TestCase(2, new[] {1, 5})]
        [TestCase(3, new[] {1, 1, 6})]
        public void FinalRest(int count, int[] rolls)
        {
            var tacticName = $"Final Rest ({count} dice)";
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("Final Rest").At("Village").Grace(3).Secrecy(7))
                .GivenLocation("Village", x => x.Blights("Corruption"))
                .WhenPlayerTakesAttackAction(x => x.Tactic(tacticName).Rolls(rolls))
                .ThenSpace("Village", x => x.NoBlights())
                .ThenHero(x => x.HasUsedAction().Grace(2).Secrecy(6));
        }

        [Test]
        public void BlindingBlack()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.HasPowers("Blinding Black").At("Swamp").Secrecy(0))
                .GivenNecromancerLocation("Castle")
                .WhenNecromancerTakesTurn(x => x.Rolls(5), player => player.UsePower("Blinding Black"))
                .ThenNecromancerLocation("Village")
                .ThenPower("Blinding Black", x => x.IsExhausted());
        }

        [Test]
        public void CallToDeath()
        {
            new TestScenario()
                .GivenLocation("Swamp", x => x.Blights("Skeletons", "Shades", "Lich"))
                .GivenHero("Acolyte", x => x.HasPowers("Call to Death").At("Swamp"))
                .WhenPlayerTakesAttackAction(x => x.Action("Call to Death").Target("Skeletons", "Lich").Rolls(5, 6))
                .WhenPlayerAssignsRolledDiceToBlights(Tuple.Create("Lich", 6), Tuple.Create("Skeletons", 5))
                .ThenSpace("Swamp", x => x.Blights("Shades"))
                .ThenHero(x => x.LostSecrecy(2).HasUsedAction());
        }

        [Test]
        public void CallToDeath_CombinedWith_FinalRest()
        {
            new TestScenario()
                .GivenLocation("Swamp", x => x.Blights("Skeletons", "Shades"))
                .GivenHero("Acolyte", x => x.HasPowers("Call to Death", "Final Rest").At("Swamp"))
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
                .GivenLocation("Swamp", x => x.Blights("Spies"))
                .GivenHero("Acolyte", x => x.HasPowers("Dark Veil").At("Swamp"))
                .WhenPlayerTakesAttackAction(player => player.UsePower("Dark Veil").Rolls(1))
                .ThenHero(x => x.HasUsedAction().LostSecrecy()) // loses Secrecy for making attack, but not for Spies defense
                .ThenPower("Dark Veil", x => x.IsExhausted());
        }

        [Test]
        public void DarkVeil_IgnoreBlightEffects()
        {
            TestScenario
                .Given.Game.WithHero("Acolyte").HasPowers("Dark Veil")
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
                .GivenLocation("Swamp", x => x.Blights("Spies"))
                .GivenHero("Acolyte", x => x.HasPowers("Death Mask").At("Swamp"))
                .WhenPlayerTakesAttackAction(player => player.Rolls(1))
                .ThenHero(x => x.HasUsedAction().LostSecrecy()); // loses Secrecy for Spies defense, but not for making attack
        }

        [Test]
        public void DeathMask_IgnoreSecrecyLossForBeingInNecromancersLocation()
        {
            TestScenario
                .Given.Game.NecromancerIn("Swamp")
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
                .WhenPlayerTakesAttackAction(x => x.Tactic("Final Rest (3 dice)").Rolls(rolls))
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
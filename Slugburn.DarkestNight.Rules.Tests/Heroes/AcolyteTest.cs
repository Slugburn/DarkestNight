﻿using System;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class AcolyteTest
    {
        [Test]
        public void BlindingBlack()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.Power("Blinding Black").Location(Location.Swamp).Secrecy(0))
                .GivenNecromancerLocation(Location.Castle)
                .WhenNecromancerRollsForMovement(5, player=>player.UsePower("Blinding Black"))
                .ThenNecromancerLocation(Location.Village)
                .ThenPower("Blinding Black", x => x.IsExhausted());
        }

        [Test]
        public void CallToDeath()
        {
            new TestScenario()
                .GivenSpace(Location.Swamp, x => x.Blight(Blight.Skeletons, Blight.Shades, Blight.Lich))
                .GivenHero("Acolyte", x => x.Power("Call to Death").Location(Location.Swamp).Secrecy(6))
                .WhenPlayerTakesFightAction(x=>x.Action("Call to Death").Target(Blight.Skeletons, Blight.Lich).Roll(5,6))
                .WhenPlayerAssignsRolledDiceToBlights(BlightDieAssignment.Create(Blight.Lich, 6), BlightDieAssignment.Create(Blight.Skeletons, 5))
                .ThenSpace(Location.Swamp, x => x.Blights(Blight.Shades))
                .ThenHero("Acolyte", x => x.Secrecy(4).HasUsedAction());
        }

        [Test]
        public void CallToDeath_CombinedWith_FinalRest()
        {
            new TestScenario()
                .GivenSpace(Location.Swamp, x => x.Blight(Blight.Skeletons, Blight.Shades))
                .GivenHero("Acolyte", x => x.Power("Call to Death", "Final Rest").Location(Location.Swamp).Secrecy(6))
                .WhenPlayerTakesAction("Call to Death")
                .GivenPlayerWillRoll(5, 2, 3, 1)
                .WhenPlayerSelectsTacticAndTarget("Final Rest (3 dice)", Blight.Skeletons, Blight.Shades)
                .WhenPlayerAcceptsRoll()
                .WhenPlayerAssignsRolledDiceToBlights(BlightDieAssignment.Create(Blight.Shades, 5), BlightDieAssignment.Create(Blight.Skeletons, 3))
                .ThenSpace(Location.Swamp, x => x.Blights(Blight.Skeletons))
                .ThenHero("Acolyte", x => x.HasUsedAction()
                .Grace(1) // loses Grace from failing to kill Skeletons and rolling a 1 with Final Rest
                .Secrecy(4)); // loses 2 Secrecy from making 2 attacks
        }

        [Test]
        public void DarkVeil_IgnoreBlightEffects()
        {
            new TestScenario()
                .GivenSpace(Location.Swamp, x => x.Blight(Blight.Spies))
                .GivenHero("Acolyte", x => x.Power("Dark Veil").Location(Location.Swamp))
                .WhenHeroUsesBonusAction("Acolyte", "Dark Veil")
                .WhenHeroEndsTurn("Acolyte")
                .ThenHero("Acolyte", x=>x.Secrecy(7))
                .ThenPower("Dark Veil", x=>x.IsExhausted());
        }

        [Test]
        public void DarkVeil_IgnoreBlightDefense()
        {
            new TestScenario()
                .GivenSpace(Location.Swamp, x => x.Blight(Blight.Spies))
                .GivenHero("Acolyte", x => x.Power("Dark Veil").Location(Location.Swamp).Secrecy(7))
                .WhenPlayerTakesAction("Attack", player=>player.UsePower("Dark Veil").Rolls(1))
                .WhenPlayerSelectsTacticAndTarget("None", Blight.Spies)
                .ThenHero("Acolyte", x => x.HasUsedAction().Secrecy(6)) // loses Secrecy for making attack, but not for Spies defense
                .ThenPower("Dark Veil", x => x.IsExhausted());
        }

        [Test]
        public void DeathMask_IgnoreSecrecyLossForAttacking()
        {
            new TestScenario()
                .GivenSpace(Location.Swamp, x => x.Blight(Blight.Spies))
                .GivenHero("Acolyte", x => x.Power("Death Mask").Location(Location.Swamp).Secrecy(7))
                .WhenPlayerTakesAction("Attack", player=>player.Rolls(1))
                .WhenPlayerSelectsTacticAndTarget("None", Blight.Spies)
                .ThenHero("Acolyte", x => x.HasUsedAction().Secrecy(6)); // loses Secrecy for Spies defense, but not for making attack
        }

        [Test]
        public void DeathMask_IgnoreSecrecyLossForBeingInNecromancersLocation()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.Power("Death Mask").Location(Location.Swamp).Secrecy(7))
                .GivenNecromancerLocation(Location.Swamp)
                .WhenHeroStartsTurn("Acolyte")
                .ThenHero("Acolyte", x => x.Secrecy(7));
        }

        [TestCase(9,0)]
        [TestCase(10,1)]
        [TestCase(19,1)]
        [TestCase(20,2)]
        public void FadeToBlack(int darkness, int bonusDice)
        {
            var expectedDice = 1 + bonusDice;
            var rolls = Enumerable.Repeat(6, expectedDice).ToArray();
            new TestScenario()
                .GivenDarkness(darkness)
                .GivenHero("Acolyte", x => x.Power("Fade to Black").Location(Location.Monastery))
                .GivenSpace(Location.Monastery, x => x.Blight(Blight.Skeletons))
                .WhenPlayerTakesAction("Attack")
                .GivenPlayerWillRoll(rolls)
                .WhenPlayerSelectsTacticAndTarget("None", Blight.Skeletons)
                .ThenPlayer(x => x.LastRoll(expectedDice));
        }

        [Test]
        public void FadeToBlack_CombinedWith_FinalRest()
        {
            var expectedDice = 5;
            var rolls = Enumerable.Repeat(6, expectedDice).ToArray();
            new TestScenario()
                .GivenDarkness(20)
                .GivenHero("Acolyte", x => x.Power("Fade to Black", "Final Rest").Location(Location.Monastery))
                .GivenSpace(Location.Monastery, x => x.Blight(Blight.Skeletons))
                .WhenPlayerTakesAction("Attack")
                .GivenPlayerWillRoll(rolls)
                .WhenPlayerSelectsTacticAndTarget("Final Rest (3 dice)", Blight.Skeletons)
                .ThenPlayer(x => x.LastRoll(expectedDice));
        }

        [Test]
        public void FalseLife()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.Power("False Life").Location(Location.Swamp).Grace(0))
                .WhenHeroUsesBonusAction("Acolyte", "False Life")
                .ThenHero("Acolyte", x => x.Grace(1))
                .ThenPower("False Life", x => x.IsExhausted());
        }

        [Test]
        public void FalseLife_NotUsableInMonastery()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.Power("False Life").Location(Location.Monastery).Grace(0))
                .ThenPowerIsUsable("False Life", false);
        }

        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        [TestCase(4, false)]
        public void FalseLife_NotUsableWhenAtOrAboveDefaultGrace(int grace, bool usable)
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.Power("False Life").Location(Location.Swamp).Grace(grace))
                .ThenPowerIsUsable("False Life", usable);
        }

        [Test]
        public void FalseLife_PreventsMovementToMonasteryWhileExhausted()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.Power("False Life").Location(Location.Village).Grace(2))
                .ThenHero("Acolyte", x => x.Grace(2).CanMoveTo(Location.Monastery))
                .WhenHeroUsesBonusAction("Acolyte", "False Life")
                .ThenHero("Acolyte", x => x.CannotMoveTo(Location.Monastery))
                .WhenPowerIsRefreshed("False Life")
                .ThenHero("Acolyte", x => x.CanMoveTo(Location.Monastery));
        }

        [Test]
        public void FalseOrders()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.Power("False Orders").Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Confusion, Blight.Corruption, Blight.Shroud, Blight.Skeletons))
                .GivenSpace(Location.Monastery, x => x.Blight(Blight.Lich))
                .WhenPlayerTakesAction("False Orders",
                    x => x.ChooseLocation(Location.Monastery)
                        .ChoosesBlight(Blight.Confusion, Blight.Corruption, Blight.Shroud))
                .ThenHero("Acolyte", x => x.HasUsedAction())
                .ThenSpace(Location.Village, x => x.Blights(Blight.Skeletons))
                .ThenSpace(Location.Monastery, x => x.Blights(Blight.Lich, Blight.Confusion, Blight.Corruption, Blight.Shroud));
        }

        [Test]
        public void FalseOrders_CancelChooseLocation()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.Power("False Orders").Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Confusion, Blight.Corruption, Blight.Shroud, Blight.Skeletons))
                .GivenSpace(Location.Monastery, x => x.Blight(Blight.Lich))
                .WhenPlayerTakesAction("False Orders", x => x.ChooseLocation(Location.None))
                .ThenHero("Acolyte", x => x.HasNotUsedAction());
        }

        [Test]
        public void FalseOrders_CancelChooseBlights()
        {
            new TestScenario()
                .GivenHero("Acolyte", x => x.Power("False Orders").Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Confusion, Blight.Corruption, Blight.Shroud, Blight.Skeletons))
                .GivenSpace(Location.Monastery, x => x.Blight(Blight.Lich))
                .WhenPlayerTakesAction("False Orders",
                    x => x.ChooseLocation(Location.Monastery).ChoosesBlight(Blight.None))
                .ThenHero("Acolyte", x => x.HasNotUsedAction());
        }

        [TestCase(2, new[] {1,5})]
        [TestCase(3, new[] {1,1,6})]
        public void FinalRest(int count, int[] rolls)
        {
            var tacticName = $"Final Rest ({count} dice)";
            new TestScenario()
                .GivenHero("Acolyte", x => x.Power("Final Rest").Location(Location.Village).Grace(3).Secrecy(7))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Corruption))
                .WhenPlayerTakesAction("Attack")
                .GivenPlayerWillRoll(rolls)
                .WhenPlayerSelectsTacticAndTarget(tacticName, Blight.Corruption)
                .ThenSpace(Location.Village, x => x.NoBlights())
                .ThenHero("Acolyte", x => x.HasUsedAction().Grace(2).Secrecy(6));
        }

        [Test]
        public void ForbiddenArts()
        {
            new TestScenario()
                .GivenDarkness(0)
                .GivenHero("Acolyte", x => x.Power("Forbidden Arts").Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Confusion))
                .WhenHeroAttacksBlight("Acolyte", player => player
                    .RollAnotherDie(true, true, false)
                    .Rolls(1, 1, 4))
                .ThenHero("Acolyte", x => x.HasUsedAction().Secrecy(6))
                .ThenDarkness(1);
        }
    }
}

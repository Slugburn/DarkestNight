using System;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class AcolyteTest
    {
        [Test]
        public void BlindingBlack()
        {
            new TestScenario()
                .GivenActingHero("Acolyte", x => x.Power("Blinding Black").Location(Location.Swamp).Secrecy(0))
                .GivenNecromancerLocation(Location.Castle)
                .WhenNecromancerTakesTurn(x => x.Rolls(5), player => player.UsePower("Blinding Black"))
                .ThenNecromancerLocation(Location.Village)
                .ThenPower("Blinding Black", x => x.IsExhausted());
        }

        [Test]
        public void CallToDeath()
        {
            new TestScenario()
                .GivenSpace(Location.Swamp, x => x.Blight(Blight.Skeletons, Blight.Shades, Blight.Lich))
                .GivenActingHero("Acolyte", x => x.Power("Call to Death").Location(Location.Swamp))
                .WhenPlayerTakesAttackAction(x=>x.Action("Call to Death").Target(Blight.Skeletons, Blight.Lich).Rolls(5,6))
                .WhenPlayerAssignsRolledDiceToBlights(Tuple.Create(Blight.Lich, 6), Tuple.Create(Blight.Skeletons, 5))
                .ThenSpace(Location.Swamp, x => x.Blights(Blight.Shades))
                .ThenHero(x => x.LostSecrecy(2).HasUsedAction());
        }

        [Test]
        public void CallToDeath_CombinedWith_FinalRest()
        {
            new TestScenario()
                .GivenSpace(Location.Swamp, x => x.Blight(Blight.Skeletons, Blight.Shades))
                .GivenActingHero("Acolyte", x => x.Power("Call to Death", "Final Rest").Location(Location.Swamp))
                .WhenPlayerTakesAttackAction(x => x.Action("Call to Death").Tactic("Final Rest (3 dice)").Target(Blight.Skeletons, Blight.Shades).Rolls(5, 2, 3, 1))
                .WhenPlayerAssignsRolledDiceToBlights(Tuple.Create(Blight.Shades, 5), Tuple.Create(Blight.Skeletons, 3))
                .ThenSpace(Location.Swamp, x => x.Blights(Blight.Skeletons))
                .ThenHero(x => x.HasUsedAction()
                    .LostGrace(2)       // loses Grace from failing to kill Skeletons and rolling a 1 with Final Rest
                    .LostSecrecy(2));   // loses 2 Secrecy from making 2 attacks
        }

        [Test]
        public void DarkVeil_IgnoreBlightEffects()
        {
            new TestScenario()
                .GivenActingHero("Acolyte", x => x.Power("Dark Veil"))
                .WhenPlayer(x=>x.TakesAction("Dark Veil"))
                .ThenHero(x=>x.IsIgnoringBlights())
                .ThenPower("Dark Veil", x => x.IsExhausted())
                .WhenHero(x=>x.StartsTurn())
                .ThenHero(x=>x.IsNotIgnoringBlights());
        }

        [Test]
        public void DarkVeil_IgnoreBlightDefense()
        {
            new TestScenario()
                .GivenSpace(Location.Swamp, x => x.Blight(Blight.Spies))
                .GivenActingHero("Acolyte", x => x.Power("Dark Veil").Location(Location.Swamp))
                .WhenPlayerTakesAttackAction(player=>player.UsePower("Dark Veil").Rolls(1))
                .ThenHero(x => x.HasUsedAction().LostSecrecy()) // loses Secrecy for making attack, but not for Spies defense
                .ThenPower("Dark Veil", x => x.IsExhausted());
        }

        [Test]
        public void DeathMask_IgnoreSecrecyLossForAttacking()
        {
            new TestScenario()
                .GivenSpace(Location.Swamp, x => x.Blight(Blight.Spies))
                .GivenActingHero("Acolyte", x => x.Power("Death Mask").Location(Location.Swamp))
                .WhenPlayerTakesAttackAction(player=>player.Rolls(1))
                .ThenHero(x => x.HasUsedAction().LostSecrecy()); // loses Secrecy for Spies defense, but not for making attack
        }

        [Test]
        public void DeathMask_IgnoreSecrecyLossForBeingInNecromancersLocation()
        {
            new TestScenario()
                .GivenActingHero("Acolyte", x => x.Power("Death Mask").Location(Location.Swamp))
                .GivenNecromancerLocation(Location.Swamp)
                .WhenHero(x=>x.StartsTurn())
                .ThenHero(x => x.LostSecrecy(0));
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
                .GivenActingHero("Acolyte", x => x.Power("Fade to Black").Location(Location.Monastery))
                .GivenSpace(Location.Monastery, x => x.Blight(Blight.Skeletons))
                .WhenPlayerTakesAttackAction(x=>x.Rolls(rolls))
                .ThenHero(x => x.FightDice(expectedDice).RolledNumberOfDice(expectedDice).HasUsedAction().LostSecrecy());
        }

        [Test]
        public void FadeToBlack_CombinedWith_FinalRest()
        {
            var rolls = Enumerable.Repeat(6, 5).ToArray();
            new TestScenario()
                .GivenDarkness(20)
                .GivenActingHero("Acolyte", x => x.Power("Fade to Black", "Final Rest").Location(Location.Monastery))
                .GivenSpace(Location.Monastery, x => x.Blight(Blight.Skeletons))
                .WhenPlayerTakesAttackAction(x=>x.Tactic("Final Rest (3 dice)").Rolls(rolls))
                .ThenHero(x => x.FightDice(3).RolledNumberOfDice(5).HasUsedAction().LostSecrecy());
        }

        [Test]
        public void FalseLife()
        {
            new TestScenario()
                .GivenActingHero("Acolyte", x => x.Power("False Life").Location(Location.Swamp).Grace(0))
                .WhenHeroUsesBonusAction("Acolyte", "False Life")
                .ThenHero(x => x.Grace(1))
                .ThenPower("False Life", x => x.IsExhausted());
        }

        [Test]
        public void FalseLife_NotUsableInMonastery()
        {
            new TestScenario()
                .GivenActingHero("Acolyte", x => x.Power("False Life").Location(Location.Monastery).Grace(0))
                .ThenHero(x=>x.Grace(0).CanUsePower("False Life", false));
        }

        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        [TestCase(4, false)]
        public void FalseLife_NotUsableWhenAtOrAboveDefaultGrace(int grace, bool usable)
        {
            new TestScenario()
                .GivenActingHero("Acolyte", x => x.Power("False Life").Location(Location.Swamp).Grace(grace))
                .ThenHero(x=>x.Grace(grace).CanUsePower("False Life", usable));
        }

        [Test]
        public void FalseLife_PreventsMovementToMonasteryWhileExhausted()
        {
            new TestScenario()
                .GivenActingHero("Acolyte", x => x.Power("False Life").Location(Location.Village).Grace(2))
                .ThenHero(x => x.Grace(2).CanMoveTo(Location.Monastery))
                .WhenHeroUsesBonusAction("Acolyte", "False Life")
                .ThenHero(x => x.CannotMoveTo(Location.Monastery))
                .WhenHero(x => x.RefreshesPower("False Life"))
                .ThenHero(x => x.CanMoveTo(Location.Monastery));
        }

        [Test]
        public void FalseOrders()
        {
            new TestScenario()
                .GivenActingHero("Acolyte", x => x.Power("False Orders").Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Confusion, Blight.Corruption, Blight.Shroud, Blight.Skeletons))
                .GivenSpace(Location.Monastery, x => x.Blight(Blight.Lich))
                .WhenPlayerTakesAction("False Orders",
                    x => x.ChooseLocation(Location.Monastery)
                        .ChoosesBlight(Blight.Confusion, Blight.Corruption, Blight.Shroud))
                .ThenHero(x => x.HasUsedAction())
                .ThenSpace(Location.Village, x => x.Blights(Blight.Skeletons))
                .ThenSpace(Location.Monastery, x => x.Blights(Blight.Lich, Blight.Confusion, Blight.Corruption, Blight.Shroud));
        }

        [Test]
        public void FalseOrders_CancelChooseLocation()
        {
            new TestScenario()
                .GivenActingHero("Acolyte", x => x.Power("False Orders").Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Confusion, Blight.Corruption, Blight.Shroud, Blight.Skeletons))
                .GivenSpace(Location.Monastery, x => x.Blight(Blight.Lich))
                .WhenPlayerTakesAction("False Orders", x => x.ChooseLocation(Location.None))
                .ThenHero(x => x.HasNotUsedAction());
        }

        [Test]
        public void FalseOrders_CancelChooseBlights()
        {
            new TestScenario()
                .GivenActingHero("Acolyte", x => x.Power("False Orders").Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Confusion, Blight.Corruption, Blight.Shroud, Blight.Skeletons))
                .GivenSpace(Location.Monastery, x => x.Blight(Blight.Lich))
                .WhenPlayerTakesAction("False Orders",
                    x => x.ChooseLocation(Location.Monastery).ChoosesBlight(Blight.None))
                .ThenHero(x => x.HasNotUsedAction());
        }

        [TestCase(2, new[] {1,5})]
        [TestCase(3, new[] {1,1,6})]
        public void FinalRest(int count, int[] rolls)
        {
            var tacticName = $"Final Rest ({count} dice)";
            new TestScenario()
                .GivenActingHero("Acolyte", x => x.Power("Final Rest").Location(Location.Village).Grace(3).Secrecy(7))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Corruption))
                .WhenPlayerTakesAttackAction(x=>x.Tactic(tacticName).Rolls(rolls))
                .ThenSpace(Location.Village, x => x.NoBlights())
                .ThenHero(x => x.HasUsedAction().Grace(2).Secrecy(6));
        }

        [Test]
        public void ForbiddenArts()
        {
            new TestScenario()
                .GivenDarkness(0)
                .GivenActingHero("Acolyte", x => x.Power("Forbidden Arts").Location(Location.Village))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Confusion))
                .WhenPlayerTakesAction("Attack")
                .WhenPlayerSelectsTactic(x=>x.Rolls(1))
                .WhenPlayerTakesAction("Forbidden Arts", x=>x.Rolls(1))
                .ThenDarkness(1)
                .WhenPlayerTakesAction("Forbidden Arts", x => x.Rolls(4))
                .WhenPlayerAcceptsRoll()
                .ThenHero(x => x.HasUsedAction().Secrecy(6));
        }

        [Test]
        public void LeechLife()
        {
            new TestScenario()
                .GivenActingHero("Acolyte", x => x.Power("Leech Life").Location(Location.Village).Grace(1))
                .GivenSpace(Location.Village, x => x.Blight(Blight.Corruption))
                .WhenPlayerTakesAttackAction(x => x.Tactic("Leech Life").Rolls(1, 5, 6))
                .ThenSpace(Location.Village, x => x.NoBlights())
                .ThenHero(x => x.HasUsedAction().Grace(2).LostSecrecy()) // two successes gains a Grace
                .ThenPower("Leech Life", x=>x.IsExhausted());
        }

        [Test]
        public void LeechLife_NotUsableInMonastery()
        {
            new TestScenario()
                .GivenActingHero("Acolyte", x => x.Power("Leech Life").Location(Location.Monastery))
                .ThenHero(x=>x.CanUsePower("Leech Life", false));
        }

        [Test]
        public void LeechLife_PreventsMovementToMonasteryWhileExhausted()
        {
            new TestScenario()
                .GivenActingHero("Acolyte", x => x.Power("Leech Life").Location(Location.Village))
                .GivenPower("Leech Life", x=>x.IsExhausted())
                .ThenHero(x => x.CannotMoveTo(Location.Monastery));
        }
    }
}

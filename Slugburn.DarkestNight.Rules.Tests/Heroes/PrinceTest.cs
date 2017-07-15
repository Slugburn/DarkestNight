using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class PrinceTest
    {
        // Chapel (Action): Spend 1 Secrecy to activate in your location. 
        // Active: Heroes may pray there.
        [Test]
        public void Chapel_Activate()
        {
            TestScenario.Game
                .WithHero("Prince").HasPowers("Chapel").At("Village")
                .When.Player.TakesAction("Chapel")
                .Then(Verify.Location("Village").HasAction("Pray [Chapel]"))
                .Then(Verify.Power("Chapel").IsActive())
                .Then(Verify.Player.Hero("Prince").LostSecrecy(1));
        }

        [Test]
        public void Chapel_UseAction()
        {
            TestScenario.Game
                .WithHero("Prince").HasPowers("Chapel").Power("Chapel").IsActive("Village")
                .WithHero("Knight").Grace(0).At("Village")
                .Given.Hero("Knight").IsTakingTurn()
                .Then(Verify.Player.Hero("Knight").Commands.Includes("Pray [Chapel]"))
                .When.Player.TakesAction("Knight", "Pray [Chapel]", Fake.Rolls(3, 3))
                .Then(Verify.Player.PrayerView.Roll(3, 3).Before(0).After(2))
                .When.Player.AcceptsRoll()
                .Then(Verify.Player.Hero("Knight").Grace(2));
        }

        // Divine Right (Bonus): +1 to default Grace. Add 1 to each die when praying.
        [Test]
        public void DivineRight()
        {
            TestScenario.Game
                .WithHero("Prince").HasPowers("Divine Right").Grace(0)
                .Then(Verify.Hero().DefaultGrace(5).Grace(0))
                .When.Player.TakesAction("Pray", Fake.Rolls(2, 3))
                .Then(Verify.Player.PrayerView.Roll(3, 4).Before(0).After(2))
                .When.Player.AcceptsRoll()
                .Then(Verify.Player.Hero("Prince").Grace(2).DefaultGrace(5));
        }

        // Inspire (Action): Activate on a hero in your location. Deactivate before any die roll for +3d.
        [Test]
        public void Inspire()
        {
            TestScenario.Game
                .WithHero("Prince").At("Mountains").HasPowers("Inspire")
                .WithHero("Knight").At("Mountains")
                .Given.Hero("Prince").IsTakingTurn()
                .When.Player.TakesAction("Inspire").SelectsHero("Knight")
                .Given.Hero("Knight").IsTakingTurn()
                .When.Player.TakesAction("Knight", "Deactivate Inspire")
                .When.Player.TakesAction("Search", Fake.Rolls(1, 2, 4, 6))
                .Then(Verify.Player.SearchView.Roll(1, 2, 4, 6))
                .When.Player.AcceptsRoll()
                .Then(Verify.Hero("Knight").FightDice(1).EludeDice(1).SearchDice(1));
        }

        // Loyalty (Bonus): +1d when eluding.
        [Test]
        public void Loyalty()
        {
            TestScenario.Game
                .WithHero("Prince").HasPowers("Loyalty")
                .Then(Verify.Hero("Prince").EludeDice(2));
        }

        // Rebellion (Tactic): Fight with 3d when attacking a blight or the Necromancer
        [Test]
        public void Rebellion_AttackingBlight()
        {
            TestScenario.Game
                .WithHero("Prince").HasPowers("Rebellion").At("Village")
                .Location("Village").HasBlights("Skeletons")
                .When.Player.TakesAction("Attack")
                .Then(Verify.Player.ConflictModel.HasTactics("Fight","Rebellion"))
                .When.Player.Targets("Skeletons").UsesTactic("Rebellion").ResolvesConflict()
                .Then(Verify.Player.ConflictModel.Rolled(6, 6, 6));
        }

        [Test]
        public void Rebellion_Necromancer()
        {
            TestScenario.Game
                .WithHero("Prince").Secrecy(0).HasPowers("Rebellion").At("Village").IsTakingTurn(false)
                .Necromancer.At("Village")
                .When.Player.TakesAction("Prince", "Start Turn")
                .Targets("Necromancer").UsesTactic("Rebellion").ResolvesConflict()
                .Then(Verify.Player.ConflictModel.Rolled(6, 6, 6));
        }

        [Test]
        public void Rebellion_Enemies()
        {
            TestScenario.Game
                .WithHero("Prince").HasPowers("Rebellion").IsFacingEnemy("Skeleton")
                .Then(Verify.Player.ConflictModel.HasTactics("Fight", "Elude"));
        }

        // Resistance (Action): Spend 1 Secrecy to activate in your location. Heroes gain +1d in fights when attacking blights there.
        [Test]
        public void Resistance_Activate()
        {
            TestScenario.Game
                .WithHero("Prince").At("Village").HasPowers("Resistance")
                .When.Player.TakesAction("Resistance")
                .Then(Verify.Power("Resistance").IsActive())
                .Then(Verify.Player.Hero("Prince").LostSecrecy(1));
        }

        [Test]
        public void Resistance_Bonus()
        {
            TestScenario.Game
                .WithHero("Prince").HasPowers("Resistance").Power("Resistance").IsActive("Village")
                .WithHero("Knight").At("Village")
                .Location("Village").HasBlights("Skeletons")
                .When.Player.TakesAction("Knight", "Attack").Targets("Skeletons").UsesTactic("Fight").ResolvesConflict()
                .Then(Verify.Player.ConflictModel.Rolled(6, 6));
        }

        [Test]
        public void Resistance_BonusDoesNotApplyToEnemies()
        {
            TestScenario.Game
                .WithHero("Prince").HasPowers("Resistance").Power("Resistance").IsActive("Village")
                .WithHero("Knight").At("Village")
                .Given.Hero("Knight").IsFacingEnemy("Skeleton")
                .When.Player.Targets("Skeleton").UsesTactic("Fight").ResolvesConflict()
                .Then(Verify.Player.ConflictModel.Rolled(6));
        }

        // Safe House (Action): Spend 2 Secrecy to activate in your location.
        //   Heroes gain 1 Secrecy (up to 5) when ending a turn there, and +1d when eluding there.
        [Test]
        public void SafeHouse_Activate()
        {
            TestScenario.Game
                .WithHero("Prince").NotAt("Monastery").HasPowers("Safe House")
                .When.Player.TakesAction("Safe House")
                .Then(Verify.Power("Safe House").IsActive())
                .Then(Verify.Player.Hero("Prince").LostSecrecy(1)); // Spent 2 but gained 1 back
        }

        [Test]
        public void SafeHouse_EndTurn()
        {
            TestScenario.Game
                .WithHero("Prince").HasPowers("Safe House").Power("Safe House").IsActive("Village")
                .WithHero("Knight").At("Village").Secrecy(0)
                .When.Player.TakesAction("Knight", "End Turn")
                .Then(Verify.Player.Hero("Knight").Secrecy(1));
        }

        [Test]
        public void SafeHouse_EludeBonus()
        {
            TestScenario.Game
                .WithHero("Prince").HasPowers("Safe House").Power("Safe House").IsActive("Village")
                .WithHero("Knight").At("Village")
                .Given.Hero("Knight").IsFacingEnemy("Skeleton")
                .When.Player.Targets("Skeleton").UsesTactic("Elude").ResolvesConflict()
                .Then(Verify.Player.ConflictModel.Rolled(6, 6));
        }

        // Scouts (Action): Spend 1 Secrecy to activate in your location.
        [Test]
        public void Scouts_Activate()
        {
            TestScenario.Game
                .WithHero("Prince").NotAt("Monastery").HasPowers("Scouts")
                .When.Player.TakesAction("Scouts")
                .Then(Verify.Power("Scouts").IsActive())
                .Then(Verify.Player.Hero("Prince").LostSecrecy(1));
        }

        [Test]
        public void Scouts_SearchBonus()
        {
            TestScenario.Game
                .WithHero("Prince").HasPowers("Scouts").Power("Scouts").IsActive("Village")
                .WithHero("Knight").At("Village")
                .When.Player.TakesAction("Knight", "Search")
                .Then(Verify.Player.SearchView.Roll(6, 6));
        }

        // Secret Passage (Action): Move to an adjacent location and gain 2 Secrecy (up to 5).
        [Test]
        public void SecretPassage()
        {
            TestScenario.Game
                .WithHero("Prince").At("Castle").Secrecy(0).HasPowers("Secret Passage")
                .When.Player.TakesAction("Secret Passage")
                .Then(Verify.Player.LocationSelectionView("Village", "Mountains", "Swamp"))
                .When.Player.SelectsLocation("Village")
                .Then(Verify.Player.Hero("Prince").Location("Village").Secrecy(2));
        }

        // Strategy (Tactic): Fight with 2d.
        [Test]
        public void Strategy()
        {
            TestScenario.Game
                .WithHero("Prince").HasPowers("Strategy")
                .Given.Hero("Prince").IsFacingEnemy("Skeleton")
                .Then(Verify.Player.ConflictModel.HasTactics("Fight", "Elude", "Strategy"))
                .When.Player.Targets("Skeleton").UsesTactic("Strategy").ResolvesConflict()
                .Then(Verify.Player.ConflictModel.Rolled(6, 6));
        }
    }
}
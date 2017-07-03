using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    [TestFixture]
    public class PriestTest
    {
        // Benediction (Action): One hero at your location gains 1 Grace (up to default). 
        // If they now have more Grace than you, you gain 1 Grace.
        [Test]
        public void Benediction()
        {
            TestScenario.Game
                .WithHero("Knight").Grace(0)
                .WithHero("Priest").Grace(0).HasPowers("Benediction")
                .When.Player.TakesAction("Benediction")
                .Then(Verify.Player.HeroSelectionView("Knight", "Priest"))
                .When.Player.SelectsHero("Knight")
                .Then(Verify.Hero("Knight").Grace(1))
                .Then(Verify.Hero("Priest").Grace(1).HasUsedAction());
        }

        [Test]
        public void Benediction_OnlyUpToDefaultGrace()
        {
            TestScenario.Game
                .WithHero("Knight")
                .WithHero("Priest").Grace(0).HasPowers("Benediction")
                .When.Player.TakesAction("Benediction")
                .Then(Verify.Player.HeroSelectionView("Priest"));
        }

        [Test]
        public void Benediction_CanNotGainGrace()
        {
            TestScenario.Game
                .WithHero("Druid").HasPowers("Sprite Form").Grace(0).Power("Sprite Form").IsActive()
                .WithHero("Priest").HasPowers("Benediction")
                .Then(Verify.Hero("Priest").CanTakeAction("Benediction", false));
        }

        // Blessing of Faith
        // Activate on a hero in your location.
        // Active: Gain an extra Grace (up to default) when praying.
        [Test]
        public void BlessingOfFaith()
        {
            TestScenario.Game
                .WithHero("Druid").At("Village")
                .WithHero("Acolyte").Grace(0).At("Monastery")
                .WithHero("Priest").HasPowers("Blessing of Faith").At("Monastery")
                .When.Player.TakesAction("Blessing of Faith")
                .Then(Verify.Player.HeroSelectionView("Acolyte","Priest"))
                .When.Player.SelectsHero("Acolyte")
                .Given.Hero("Acolyte").IsTakingTurn()
                .When.Player.TakesAction("Pray", Fake.Rolls(1, 1)).AcceptsRoll()
                .Then(Verify.Hero("Acolyte").Grace(1).HasUsedAction());
        }

        [Test]
        public void BlessingOfFaith_Exhausted()
        {
            TestScenario.Game
                .WithHero("Acolyte").Grace(0).At("Monastery")
                .WithHero("Priest").HasPowers("Blessing of Faith").At("Monastery")
                .When.Player.TakesAction("Priest", "Blessing of Faith").SelectsHero("Acolyte")
                .Given.Hero("Priest").Power("Blessing of Faith").IsExhausted()
                .Given.Hero("Acolyte").IsTakingTurn()
                .When.Player.TakesAction("Pray", Fake.Rolls(1, 1)).AcceptsRoll()
                .Then(Verify.Hero("Acolyte").Grace(0).HasUsedAction());
        }

        // Blessing of Piety
        // Activate on a hero in your location.
        // Active: Gain 1 Grace (up to default) when hiding.
        [Test]
        public void BlessingOfPiety()
        {
            TestScenario.Game
                .WithHero("Druid").Grace(0).At("Village")
                .WithHero("Priest").HasPowers("Blessing of Piety").At("Village")
                .When.Player.TakesAction("Priest", "Blessing of Piety").SelectsHero("Druid")
                .Given.Hero("Druid").IsTakingTurn()
                .When.Player.TakesAction("Hide")
                .Then(Verify.Hero("Druid").Grace(1).HasUsedAction());
        }

        // Blessing of Strength
        // Activate on a hero in your location.
        // Active: +1d in fights.
        [Test]
        public void BlessingOfStrength()
        {
            TestScenario.Game
                .WithHero("Knight").At("Village")
                .WithHero("Priest").HasPowers("Blessing of Strength").At("Village")
                .When.Player.TakesAction("Priest", "Blessing of Strength").SelectsHero("Knight")
                .Then(Verify.Hero("Knight").FightDice(2));
        }

        [Test]
        public void BlessingOfStrength_Exhausted()
        {
            TestScenario.Game
                .WithHero("Knight").At("Village")
                .WithHero("Priest").HasPowers("Blessing of Strength").At("Village")
                .When.Player.TakesAction("Priest", "Blessing of Strength").SelectsHero("Knight")
                .Given.Hero("Priest").Power("Blessing of Strength").IsExhausted()
                .Then(Verify.Hero("Knight").FightDice(1));
        }

        // Blessing of Wisdom
        // Activate on a hero in your location.
        // Active: +1d when eluding.
        [Test]
        public void BlessingOfWisdom()
        {
            TestScenario.Game
                .WithHero("Acolyte").At("Village")
                .WithHero("Priest").HasPowers("Blessing of Wisdom").At("Village")
                .When.Player.TakesAction("Priest", "Blessing of Wisdom").SelectsHero("Acolyte")
                .Then(Verify.Hero("Acolyte").EludeDice(2));
        }

        // Calm (Bonus)
        // Heroes at your location may pray.
        [Test]
        public void Calm()
        {
            TestScenario.Game
                .WithHero("Knight").At("Mountains")
                .WithHero("Priest").At("Mountains").HasPowers("Calm")
                .Then(Verify.Location("Mountains").HasAction("Pray [Calm]"))
                .Given.Hero("Knight").IsTakingTurn()
                .Then(Verify.Hero("Knight").CanTakeAction("Pray [Calm]"));
        }

        [Test]
        public void CalmFollowsPriest()
        {
            TestScenario.Game
                .WithHero("Priest").At("Mountains").HasPowers("Calm")
                .Then(Verify.Location("Mountains").HasAction("Pray [Calm]"))
                .When.Player.TakesAction("Travel").SelectsLocation("Village")
                .Then(Verify.Location("Village").HasAction("Pray [Calm]"))
                .Then(Verify.Location("Mountains").DoesNotHaveAction("Pray [Calm]"));
        }

        // Censure (Tactic)
        // Fight with 2d.
        [Test]
        public void Censure()
        {
            TestScenario.Game
                .WithHero("Priest").HasPowers("Censure").IsFacingEnemy("Skeleton")
                .When.Player.CompletesConflict("Skeleton", "Censure")
                .Then(Verify.Hero().RolledNumberOfDice(2));
        }

        // Intercession (Bonus)
        // Whenever a hero at your location loses or spends Grace, they may spend your Grace instead.
        [TestCase("Yes")]
        [TestCase("No")]
        public void Intercession_Spend(string answer)
        {
            var expectedPriestLoss = answer == "Yes" ? 1 : 0;
            var expectedKnightLoss = answer == "Yes" ? 0 : 1;
            TestScenario.Game
                .WithHero("Knight").At("Village")
                .WithHero("Priest").At("Village").HasPowers("Intercession")
                .Given.Hero("Knight").HasDrawnEvent("Latent Spell")
                .When.Player.SelectsEventOption("Spend Grace")
                .When.Player.AnswersQuestion("Intercession", answer)
                .Then(Verify.Hero("Priest").LostGrace(expectedPriestLoss))
                .Then(Verify.Hero("Knight").LostGrace(expectedKnightLoss).HasUnresolvedEvents(1));
        }

        [Test]
        public void Intercession_AutomaticSpend()
        {
            TestScenario.Game
                .WithHero("Knight").At("Village").Grace(0)
                .WithHero("Priest").At("Village").HasPowers("Intercession")
                .Given.Hero("Knight").HasDrawnEvent("Latent Spell")
                .When.Player.SelectsEventOption("Spend Grace")
                .Then(Verify.Hero("Priest").LostGrace());
        }

        [TestCase("Yes")]
        [TestCase("No")]
        public void Intercession_LostGrace(string answer)
        {
            var expectedPriestLoss = answer == "Yes" ? 1 : 0;
            var expectedKnightLoss = answer == "Yes" ? 0 : 1;
            TestScenario.Game
                .WithHero("Knight").HasPowers("Reckless Abandon")
                .WithHero("Priest").HasPowers("Intercession")
                .Given.Hero("Knight").IsTakingTurn().IsFacingEnemy("Vampire")
                .When.Player.CompletesConflict("Vampire", "Reckless Abandon", Fake.Rolls(1, 2, 3, 4))
                .When.Player.AnswersQuestion("Intercession", answer)
                .Then(Verify.Hero("Knight").LostGrace(expectedKnightLoss))
                .Then(Verify.Hero("Priest").LostGrace(expectedPriestLoss));
        }


        // Miracle (Bonus)
        // Spend 1 Grace to reroll any die roll you make. You may do this repeatedly.
        [Test]
        public void Miracle()
        {
            TestScenario
                .Game.WithHero("Priest").HasPowers("Miracle")
                .Given.Hero().HasDrawnEvent("Twist of Fate")
                .Then(Verify.Player.EventView.HasBody("Twist of Fate", 1, "Roll 1d and take the highest").HasOptions("Roll"))
                .When.Player.SelectsEventOption("Roll", Fake.Rolls(1))
                .Then(Verify.Player.EventView.ActiveRow("-1d (to a minimum of 1d) on all rolls for the rest of this turn"))
                .When.Player.TakesAction("Miracle", Fake.Rolls(6))
                .Then(Verify.Player.EventView.ActiveRow("+1d on all rolls for the rest of this turn"))
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Hero().LostGrace(1).HasDieModifier("Twist of Fate").FightDice(2).EludeDice(2).SearchDice(2));
        }

        // Sanctuary (Tactic)
        // Elude with 4d. Lose 1 Secrecy if you succeed.
        [Test]
        public void Sanctuary()
        {
            TestScenario
                .Game.WithHero("Priest").HasPowers("Sanctuary")
                .Given.Hero().IsFacingEnemy("Zombie")
                .When.Player.CompletesConflict("Zombie", "Sanctuary", Fake.Rolls(1,2,3,4))
                .Then(Verify.Hero().RolledNumberOfDice(4).LostSecrecy());
        }

        [Test]
        public void Sanctuary_Failure()
        {
            TestScenario
                .Game.WithHero("Priest").HasPowers("Sanctuary")
                .Given.Hero().IsFacingEnemy("Zombie")
                .When.Player.CompletesConflict("Zombie", "Sanctuary", Fake.Rolls(1, 2, 1, 2))
                .Then(Verify.Hero().RolledNumberOfDice(4).WasWounded());
        }
    }
}
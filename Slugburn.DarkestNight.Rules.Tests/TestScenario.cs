using System;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class TestScenario
    {
        private readonly Game _game;
        private readonly FakePlayer _player;

        public TestScenario()
        {
            _game = new Game();
            _player = new FakePlayer();
            _game.AddPlayer(_player);
        }

        public TestScenario GivenHero(string name, Action<HeroDefContext> def) 
        {
            var hero = new HeroFactory().Create(name);
            _game.AddHero(hero, _player);
            var ctx = new HeroDefContext(hero);
            def(ctx);
            _game.ActingHero = hero;
            return this;
        }

        public TestScenario GivenNecromancerLocation(Location location)
        {
            _game.Necromancer.Location = location;
            return this;
        }

        public TestScenario WhenNecromancerRollsForMovement(int value, Action<PlayerActionContext> actions = null)
        {
            actions?.Invoke(new PlayerActionContext(_player));
            _player.AddUpcomingRoll(value);
            _game.Necromancer.TakeTurn();
            return this;
        }

        public TestScenario ThenNecromancerLocation(Location location)
        {
            Assert.That(_game.Necromancer.Location, Is.EqualTo(location));
            return this;
        }

        public TestScenario ThenPower(string name, Action<PowerExpectation> expect)
        {
            var power = _game.GetPower(name);
            var expectation = new PowerExpectation(power);
            expect(expectation);
            expectation.Verify();
            return this;
        }

        public TestScenario GivenSpace(Location location, Action<SpaceDefContext> def)
        {
            var space = _game.Board[location];
            var context = new SpaceDefContext(space);
            def(context);
            return this;
        }

        public TestScenario WhenHeroEndsTurn(string heroName)
        {
            var hero = _game.GetHero(heroName);
            hero.EndTurn();
            return this;
        }

        public TestScenario ThenHero(string heroName, Action<HeroExpectation> expect)
        {
            var hero = _game.GetHero(heroName);
            var expectation = new HeroExpectation(hero);
            expect(expectation);
            expectation.Verify();
            return this;
        }

        public TestScenario WhenHeroUsesBonusAction(string heroName, string powerName)
        {
            var hero = _game.GetHero(heroName);
            var bonusAction = (IBonusAction) hero.Powers.Single(p => p.Name == powerName);
            bonusAction.Use(hero);
            return this;
        }

        public TestScenario ThenPowerIsUsable(string powerName, bool expected=true)
        {
            var power = _game.GetPower(powerName);
            Assert.That(() => power.IsUsable(_game.ActingHero), Is.EqualTo(expected));
            return this;
        }

        public TestScenario WhenPowerIsRefreshed(string powerName)
        {
            var power = _game.GetPower(powerName);
            power.Refresh();
            return this;
        }

        public TestScenario ThenSpace(Location location, Action<SpaceExpectation> define)
        {
            var space = _game.Board[location];
            var expectation = new SpaceExpectation(space);
            define(expectation);
            expectation.Verify();
            return this;
        }

        public TestScenario WhenPlayerTakesAction(string actionName, Action<PlayerActionContext> actions = null)
        {
            actions?.Invoke(new PlayerActionContext(_player));
            var hero = _game.ActingHero;
            var action = hero.GetAction(actionName);
            hero.TakeAction(action);
            return this;
        }

        public TestScenario WhenPlayerTakesAttackAction(Action<FightContext> actions)
        {
            var context=  new FightContext(_player, _game.ActingHero);
            actions(context);
            WhenPlayerTakesAction(context.GetAction());
            WhenPlayerSelectsTactic(context.GetTactic(), context.GetTargets());
            WhenPlayerAcceptsRoll();
            return this;
        }

        public TestScenario WhenHeroStartsTurn(string heroName)
        {
            var hero = _game.GetHero(heroName);
            hero.StartTurn();
            return this;
        }

        public TestScenario ThenPlayer(Action<PlayerExpectation> expect)
        {
            var expectation = new PlayerExpectation(_player);
            expect(expectation);
            expectation.Verify();
            return this;
        }

        public TestScenario GivenDarkness(int darkness)
        {
            _game.Darkness = darkness;
            return this;
        }

        public TestScenario ThenDarkness(int expected)
        {
            Assert.That(_game.Darkness, Is.EqualTo(expected));
            return this;
        }

        public TestScenario WhenPlayerSelectsTactic(string tactic, params Blight[] blights)
        {
            _game.ActingHero.SelectTactic(tactic, blights);
            return this;
        }

        public TestScenario WhenPlayerSelectsTactic(Action<TacticContext> define = null)
        {
            var context = new TacticContext(_player, _game.ActingHero);
            define?.Invoke(context);
            _game.ActingHero.SelectTactic(context.GetTactic(), context.GetTargets());
            return this;
        }

        public TestScenario WhenPlayerAssignsRolledDiceToBlights( params BlightDieAssignment[]  assignments)
        {
            _game.ActingHero.AssignDiceToBlights(assignments);
            return this;
        }

        public TestScenario GivenPlayerWillRoll(params int[] rolls)
        {
            _player.AddUpcomingRolls(rolls);
            return this;
        }

        public TestScenario WhenPlayerAcceptsRoll()
        {
            _game.ActingHero.EndCombat();
            return this;
        }

        public TestScenario GivenPowerIsExhausted(string powerName)
        {
            var power = _game.GetPower(powerName);
            power.Exhaust();
            return this;
        }

        public TestScenario WhenPlayerEludes(Action<TacticContext> actions)
        {
            WhenPlayerDefends();
            WhenPlayerSelectsTactic(actions);
            WhenPlayerAcceptsRoll();
            return this;
        }

        private TestScenario WhenPlayerDefends()
        {
            var hero = _game.ActingHero;
            new Defend().Act(hero);
            return this;
        }
    }
}
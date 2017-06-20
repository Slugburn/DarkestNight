using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class TestScenario
    {
        private readonly Game _game;
        private readonly FakePlayer _player;
        private readonly FakeDie _die;

        public TestScenario()
        {
            _game = new Game();
            _player = new FakePlayer();
            _die = new FakeDie();
            Die.SetImplementation(_die);
            _game.AddPlayer(_player);
        }

        public TestScenario GivenHero(string name, Action<HeroContext> def) 
        {
            var hero = HeroFactory.Create(name);
            _game.AddHero(hero, _player);
            var ctx = new HeroContext(hero);
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
            actions?.Invoke(new PlayerActionContext(_player, _die));
            _die.AddUpcomingRoll(value);
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
            return ThenHero(hero, expect);
        }

        public TestScenario ThenHero(Action<HeroExpectation> expect)
        {
            var hero = _game.ActingHero;
            return ThenHero(hero, expect);
        }

        private TestScenario ThenHero(Hero hero, Action<HeroExpectation> expect)
        {
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
            actions?.Invoke(new PlayerActionContext(_player, _die));
            var hero = _game.ActingHero;
            var action = hero.GetAction(actionName);
            hero.TakeAction(action);
            return this;
        }

        public TestScenario WhenPlayerTakesAttackAction(Action<FightContext> actions = null)
        {
            var context=  new FightContext(_die, _player, _game.ActingHero);
            actions?.Invoke(context);
            WhenPlayerTakesAction(context.GetAction());
            WhenPlayerSelectsTactic(context.GetTactic(), context.GetTargets());
            WhenPlayerAcceptsRoll();
            return this;
        }

        public TestScenario WhenHeroStartsTurn(string heroName)
        {
            var hero = _game.GetHero(heroName);
            hero.IsActionAvailable = true;
            hero.StartTurn();
            return this;
        }

        public TestScenario ThenPlayer(Action<PlayerExpectation> expect)
        {
            var expectation = new PlayerExpectation(_player, _game);
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

        public TestScenario WhenPlayerSelectsTactic(string tactic, params string[] blights)
        {
            var targetIds = GetTargetIds(blights).ToList();
            _game.ActingHero.SelectTactic(tactic, targetIds);
            return this;
        }

        private IEnumerable<int> GetTargetIds(ICollection<string> targets)
        {
            var source = _game.ActingHero.ConflictState.AvailableTargets.ToList();
            Assert.That(source.Select(x=>x.Name).Intersect(targets), Is.EquivalentTo(targets));
            foreach (var match in targets.Select(target => source.First(x => x.Name == target)))
            {
                source.Remove(match);
                yield return match.Id;
            }
        }

        public TestScenario WhenPlayerSelectsTactic(Action<TacticContext> define = null, string defaultTactic = "Fight")
        {
            var context = new TacticContext(_game.ActingHero, _die, defaultTactic);
            define?.Invoke(context);
            var blights = context.GetTargets();
            var targetIds = GetTargetIds(blights).ToList();
            _game.ActingHero.SelectTactic(context.GetTactic(), targetIds);
            return this;
        }

        public TestScenario WhenPlayerAssignsRolledDiceToBlights( params BlightDieAssignment[]  assignments)
        {
            _game.ActingHero.AssignDiceToBlights(assignments);
            return this;
        }

        public TestScenario GivenPlayerWillRoll(params int[] rolls)
        {
            _die.AddUpcomingRolls(rolls);
            return this;
        }

        public TestScenario WhenPlayerAcceptsRoll()
        {
            _game.ActingHero.EndCombat();
            return this;
        }

        public TestScenario GivenPower(string powerName, Action<PowerContext> actions)
        {
            var hero = _game.ActingHero;
            var power = hero.GetPower(powerName);
            var context = new PowerContext(hero, power);
            actions(context);
            return this;
        }

        public TestScenario WhenHeroFights(Action<TacticContext> actions)
        {
            WhenHeroDefends();
            WhenPlayerSelectsTactic(actions, "Fight");
            WhenPlayerAcceptsRoll();
            return this;
        }

        public TestScenario WhenHeroEludes(Action<TacticContext> actions)
        {
            WhenHeroDefends();
            WhenPlayerSelectsTactic(actions, "Elude");
            WhenPlayerAcceptsRoll();
            return this;
        }

        private TestScenario WhenHeroDefends()
        {
            var hero = _game.ActingHero;
            var enemies = hero.GetBlights().GenerateEnemies();
            hero.Enemies = enemies; 
            new Defend().Act(hero);
            return this;
        }

        public TestScenario WhenPlayerSelectsLocation(Location location)
        {
            var hero = _game.ActingHero;
            hero.SelectLocation(location);
            return this;
        }

        public TestScenario ThenAvailableActions(params string[] actionNames)
        {
            var hero = _game.ActingHero;
            Assert.That(hero.AvailableActions, Is.Not.Null, "Hero.AvailableActions has not been specified.");
            Assert.That(hero.AvailableActions, Is.EquivalentTo(actionNames));
            return this;
        }

        public TestScenario WhenHeroDrawsEvent()
        {
            var hero = _game.ActingHero;
            hero.DrawEvent();
            return this;
        }

        public TestScenario ThenEventHasOption(string option, bool expected = true)
        {
            var hero = _game.ActingHero;
            var constraint = expected ? Has.Member(option) : Has.No.Member(option);
            Assert.That(hero.CurrentEvent.Options.Select(x=>x.Code), constraint, $"Event option {option} not found.");
            return this;
        }

        public TestScenario WhenPlayerSelectsEventOption(string option)
        {
            var hero = _game.ActingHero;
            hero.SelectEventOption(option);
            return this;
        }

        public TestScenario GivenNextEventIs(string eventName)
        {
            _game.Events.Insert(0, eventName);
            return this;
        }

        public TestScenario WhenBlightIsDestroyed(Location location, Blight blight)
        {
            _game.DestroyBlight(location, blight);
            return this;
        }

        public TestScenario WhenHeroMovesTo(Location location)
        {
            _game.ActingHero.MoveTo(location);
            return this;
        }

    }
}
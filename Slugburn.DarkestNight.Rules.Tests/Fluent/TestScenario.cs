using System;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public class TestScenario : IFakeContext
    {
        private readonly Game _game;
        private readonly FakePlayer _player;

        static TestScenario()
        {
            Die.Implementation = new FakeDie();
        }

        public TestScenario()
        {
            _game = new Game();
            _player = new FakePlayer(_game);
            _game.AddPlayer(_player);
        }

        public static GivenContext Given => CreateRootGiven();

        private static GivenContext CreateRootGiven()
        {
            var game = new Game();
            var player = new FakePlayer(game);
            game.AddPlayer(player);
            var given = new GivenContext(game, player);
            return given;
        }

        public TestScenario GivenHero(string name, Action<HeroContext> def = null)
        {
            AddHero(HeroFactory.Create(name), def);
            return this;
        }

        private void AddHero(Hero hero, Action<HeroContext> def)
        {
            _game.AddHero(hero, _player);
            var ctx = new HeroContext(_game, _player, hero);
            def?.Invoke(ctx);
            _game.ActingHero = hero;
            _player.ActiveHero = hero.Name;
        }

        public TestScenario ThenPower(string name, Action<PowerExpectation> expect)
        {
            var power = _game.GetPower(name);
            var expectation = new PowerExpectation(power);
            expect(expectation);
            expectation.Verify();
            return this;
        }

        public TestScenario GivenLocation(string location, Action<LocationContext> def)
        {
            var space = _game.Board[location.ToEnum<Location>()];
            var context = new LocationContext(_game, _player, space);
            def(context);
            return this;
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

        public TestScenario ThenSpace(string location, Action<LocationExpectation> define)
        {
            var space = _game.Board[location.ToEnum<Location>()];
            var expectation = new LocationExpectation(space);
            define(expectation);
            expectation.Verify();
            return this;
        }

        public TestScenario WhenPlayerTakesAction(string actionName, Action<PlayerActionContext> actions = null)
        {
            actions?.Invoke(new PlayerActionContext(_game, _player));
            var hero = _game.ActingHero;
            var action = hero.GetAction(actionName);
            hero.TakeAction(action);
            return this;
        }

        public TestScenario GivenPower(string powerName, Action<PowerContext> actions)
        {
            var hero = _game.ActingHero;
            var power = hero.GetPower(powerName);
            var context = new PowerContext(_game, _player, hero, power);
            actions(context);
            return this;
        }

        public TestScenario WhenHero(Action<IHeroActionContext> action)
        {
            var context = new HeroActionContext(_game, _player);
            action(context);
            return this;
        }
    }
}
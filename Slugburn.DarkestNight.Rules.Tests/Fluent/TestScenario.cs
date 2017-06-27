using System;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
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
            Hero hero = HeroFactory.Create(name);
            _game.AddHero(hero, _player);
            var ctx = new HeroContext(_game, _player, hero);
            def?.Invoke(ctx);
            _game.ActingHero = hero;
            _player.ActiveHero = hero.Name;
            return this;
        }

        public TestScenario ThenHero(Action<HeroExpectation> expect)
        {
            var hero = _game.ActingHero;
            var expectation = new HeroExpectation(hero);
            expect(expectation);
            expectation.Verify();
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
    }
}
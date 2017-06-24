using System;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class PlayerExpectation : Then, IPlayerExpectation
    {
        public PlayerExpectation(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IPlayerExpectation SeesEvent(string title, string text, int fate, params string[] options)
        {
            Assert.That(_player.State, Is.EqualTo(PlayerState.Event));
            var e = _player.Event;
            Assert.That(e.Title, Is.EqualTo(title));
            Assert.That(e.Text, Is.EqualTo(text));
            Assert.That(e.Fate, Is.EqualTo(fate));
            Assert.That(e.Options.Select(x => x.Text), Is.EquivalentTo(options));
            return this;
        }

        public IPlayerEventExpectation Event => new PlayerEventExpectation(_game, _player);

        public IPlayerExpectation SeesTarget(params string[] targetNames)
        {
            Assert.That(_player.State, Is.EqualTo(PlayerState.Conflict));
            var actual = _player.Conflict.Targets.Select(x => x.Name);
            Assert.That(actual, Is.EquivalentTo(targetNames));
            return this;
        }

        public IPlayerExpectation SeesTactics(params string[] tacticNames)
        {
            Assert.That(_player.State, Is.EqualTo(PlayerState.Conflict));
            var actual = _player.Conflict.Tactics.Select(x => x.Name);
            Assert.That(actual, Is.EquivalentTo(tacticNames));
            return this;
        }

        public IPlayerExpectation Powers(params string[] powerNames)
        {
            _player.State.ShouldBe(PlayerState.SelectPower);
            _player.Powers.Select(x => x.Name).ShouldBe(powerNames);
            return this;
        }

        public IPlayerExpectation Conflict(Action<PlayerConflictExpectation> expect)
        {
            var expectation = new PlayerConflictExpectation(_player.Conflict);
            expect(expectation);
            return this;
        }

        public IPlayerExpectation BlightSelectionView(Action<PlayerBlightExpectation> expect)
        {
            var expectation = new PlayerBlightExpectation(_player.Blights);
            expect(expectation);
            expectation.Verify();
            return this;
        }

        public IPlayerExpectation LocationSelectionView(params string[] locations)
        {
            _player.State.ShouldBe(PlayerState.SelectLocation);
            var actual = _player.ValidLocations.OrderBy(x => x);
            var expected = locations.OrderBy(x => x);
            actual.ShouldBe(expected);
            return this;
        }

        public void Verify()
        {
        }
    }
}
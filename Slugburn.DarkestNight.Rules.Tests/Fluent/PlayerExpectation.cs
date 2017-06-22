using System;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public class PlayerExpectation
    {
        private readonly FakePlayer _player;

        public PlayerExpectation(FakePlayer player)
        {
            _player = player;
        }

        public void Verify()
        {
        }

        public PlayerExpectation SeesEvent(string title, string text, int fate, params string[] options)
        {
            Assert.That(_player.State, Is.EqualTo(PlayerState.Event));
            var e = _player.Event;
            Assert.That(e.Title, Is.EqualTo(title));
            Assert.That(e.Text, Is.EqualTo(text));
            Assert.That(e.Fate, Is.EqualTo(fate));
            Assert.That(e.Options.Select(x=>x.Text), Is.EquivalentTo(options));
            return this;
        }

        public PlayerExpectation Event(Action<PlayerEventExpectation> expect)
        {
            Assert.That(_player.State, Is.EqualTo(PlayerState.Event));
            var expectation = new PlayerEventExpectation(_player.Event);
            expect(expectation);
            return this;
        }

        public PlayerExpectation SeesActiveEventRow(int min, int max, string text, string subText = null)
        {
            Assert.That(_player.State, Is.EqualTo(PlayerState.Event));
            var e = _player.Event;
            Assert.That(e.Rows, Is.Not.Null, "No event rows");
            var row = e.Rows.SingleOrDefault(r => r.Min == min && r.Max == max);
            if (row==null)
                Assert.Fail();
            Assert.That(row.IsActive, Is.True, "Event row is not active");
            Assert.That(row.Text, Is.EqualTo(text));
            row.SubText.ShouldBe(subText);
            return this;
        }

        public PlayerExpectation SeesTarget(params string[] targetNames)
        {
            Assert.That(_player.State, Is.EqualTo(PlayerState.Conflict));
            var actual = _player.Conflict.Targets.Select(x => x.Name);
            Assert.That(actual, Is.EquivalentTo(targetNames));
            return this;
        }

        public PlayerExpectation SeesTactics(params string[] tacticNames)
        {
            Assert.That(_player.State, Is.EqualTo(PlayerState.Conflict));
            var actual = _player.Conflict.Tactics.Select(x => x.Name);
            Assert.That(actual, Is.EquivalentTo(tacticNames));
            return this;
        }

        public PlayerExpectation Powers(params string[] powerNames)
        {
            _player.State.ShouldBe(PlayerState.SelectPower);
            _player.Powers.Select(x=>x.Name).ShouldBe(powerNames);
            return this;
        }
    }
}
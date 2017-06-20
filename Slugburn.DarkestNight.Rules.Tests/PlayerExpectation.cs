using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Tests
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

        public PlayerExpectation SeesEvent(string title, int fate, params string[] options)
        {
            Assert.That(_player.State, Is.EqualTo(PlayerState.Event));
            var e = _player.Event;
            Assert.That(e.Title, Is.EqualTo(title));
            Assert.That(e.Fate, Is.EqualTo(fate));
            Assert.That(e.Options.Select(x=>x.Text), Is.EquivalentTo(options));
            return this;
        }
    }
}
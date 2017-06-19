using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class PlayerExpectation
    {
        private readonly FakePlayer _player;
        private readonly Game _game;
        private int _expectedDice;

        public PlayerExpectation(FakePlayer player, Game game)
        {
            _player = player;
            _game = game;
        }

        public void Verify()
        {
            Assert.That(_game.ActingHero.Roll.Count, Is.EqualTo(_expectedDice));
        }

        public PlayerExpectation RolledNumberOfDice(int expectedDice)
        {
            _expectedDice = expectedDice;
            return this;
        }
    }
}
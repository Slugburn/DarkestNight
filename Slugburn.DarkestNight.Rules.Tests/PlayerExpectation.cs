using NUnit.Framework;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class PlayerExpectation
    {
        private readonly FakePlayer _player;
        private int _expectedDice;

        public PlayerExpectation(FakePlayer player)
        {
            _player = player;
        }

        public void Verify()
        {
            Assert.That(_player.GetLastRoll().Count, Is.EqualTo(_expectedDice));
        }

        public PlayerExpectation RolledNumberOfDice(int expectedDice)
        {
            _expectedDice = expectedDice;
            return this;
        }
    }
}
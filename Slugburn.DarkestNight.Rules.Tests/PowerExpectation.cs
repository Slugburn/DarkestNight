using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class PowerExpectation
    {
        private readonly IPower _power;
        private bool _expectExhausted;

        public PowerExpectation(IPower power)
        {
            _power = power;
        }

        public PowerExpectation IsExhausted()
        {
            _expectExhausted = true;
            return this;
        }

        public void Verify()
        {
            Assert.That(_power.Exhausted, Is.EqualTo(_expectExhausted), "Unexpected Exhausted value.");
        }
    }
}
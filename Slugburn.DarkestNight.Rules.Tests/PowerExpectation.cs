using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class PowerExpectation
    {
        private readonly IPower _power;
        private bool _expectExhausted;
        private bool _expectActive;

        public PowerExpectation(IPower power)
        {
            _power = power;
        }

        public void Verify()
        {
            Assert.That(_power.Exhausted, Is.EqualTo(_expectExhausted), $"Unexpected Exhausted value for {_power.Name}.");
            var activateablePower = _power as ActivateablePower;
            if (activateablePower != null)
                Assert.That(activateablePower.IsActive, Is.EqualTo(_expectActive), $"Unexpected IsActive value for {_power.Name}.");
        }

        public PowerExpectation IsExhausted(bool expected = true)
        {
            _expectExhausted = expected;
            return this;
        }

        public PowerExpectation IsActive(bool expected = true)
        {
            _expectActive = expected;
            return this;
        }
    }
}
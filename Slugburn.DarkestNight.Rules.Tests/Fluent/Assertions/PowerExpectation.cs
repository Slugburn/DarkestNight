using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class PowerExpectation
    {
        private readonly IPower _power;
        private bool _expectActive;

        public PowerExpectation(IPower power)
        {
            _power = power;
        }

        public void Verify()
        {
            var activateablePower = _power as ActivateablePower;
            if (activateablePower != null)
                Assert.That(activateablePower.IsActive, Is.EqualTo(_expectActive), $"Unexpected IsActive value for {_power.Name}.");
        }

        public PowerExpectation IsActive(bool expected = true)
        {
            _expectActive = expected;
            return this;
        }
    }
}
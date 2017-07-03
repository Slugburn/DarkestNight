using Shouldly;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class PowerVerification : IVerifiable
    {
        private readonly string _powerName;
        private bool _exhausted;
        private bool _isActive;

        public IVerifiable Parent => null;

        public PowerVerification(string powerName) 
        {
            _powerName = powerName;
        }

        public void Verify(ITestRoot root)
        {
            var game = ((TestRoot) root).GetGame();
            var power = game.GetPower(_powerName);
            power.Exhausted.ShouldBe(_exhausted);
            var activateable = power as IActivateable;
            activateable?.IsActive.ShouldBe(_isActive);
        }

        public PowerVerification IsExhausted(bool expected = true)
        {
            _exhausted = expected;
            return this;
        }

        public PowerVerification IsActive(bool expected = true)
        {
            _isActive = expected;
            return this;
        }
    }
}
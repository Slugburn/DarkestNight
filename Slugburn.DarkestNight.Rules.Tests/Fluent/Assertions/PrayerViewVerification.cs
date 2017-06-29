using Shouldly;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class PrayerViewVerification : IVerifiable
    {
        private int[] _roll;
        private int _before;
        private int _after;

        public void Verify(ITestRoot root)
        {
            var view = root.Get<FakePlayer>().Prayer;
            view.ShouldNotBeNull();
            view.Roll.ShouldBeEquivalent(_roll);
            view.Before.ShouldBe(_before);
            view.After.ShouldBe(_after);
        }

        public PrayerViewVerification Roll(params int[] expected)
        {
            _roll = expected;
            return this;
        }
        public PrayerViewVerification Before(int expected)
        {
            _before = expected;
            return this;
        }
        public PrayerViewVerification After(int expected)
        {
            _after = expected;
            return this;
        }

    }
}
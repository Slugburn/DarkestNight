using Shouldly;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class HeroEventVerification : ChildVerification
    {
        private bool? _canBeIgnored;

        public HeroEventVerification(IVerifiable parent) : base(parent)
        {
        }

        public HeroEventVerification CanBeIgnored(bool expected = true)
        {
            _canBeIgnored = expected;
            return this;
        }

        public override void Verify(ITestRoot root)
        {
            var r = (TestRoot)root;
            var hero = r.GetGame().ActingHero;
            var e = hero?.CurrentEvent;
            if (e == null) return;
            if (_canBeIgnored.HasValue)
                e.IsIgnorable.ShouldBe(_canBeIgnored.Value);
        }
    }
}
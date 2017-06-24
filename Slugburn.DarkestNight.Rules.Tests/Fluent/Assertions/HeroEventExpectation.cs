using Shouldly;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class HeroEventExpectation
    {
        private readonly Hero _hero;
        private int _expectedOutstandingEvents;
        private bool _expectIsIgnorable = true;

        public HeroEventExpectation(Hero hero)
        {
            _hero = hero;
        }

        public HeroEventExpectation HasOutstanding(int count)
        {
            _expectedOutstandingEvents = count;
            return this;
        }

        public HeroEventExpectation CanBeIgnored(bool expected = true)
        {
            _expectIsIgnorable = expected;
            return this;
        }

        public void Verify()
        {
            var outstandingEvents = (_hero.CurrentEvent != null ? 1 : 0) + _hero.EventQueue.Count;
            outstandingEvents.ShouldBe(_expectedOutstandingEvents);
            if (_hero.CurrentEvent == null) return;
            _hero.CurrentEvent.IsIgnorable.ShouldBe(_expectIsIgnorable, _hero.CurrentEvent.Name);
        }
    }
}
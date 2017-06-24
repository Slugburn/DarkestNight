using Shouldly;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public class GameVerification : IVerifiable
    {
        private int _darkness;
        private Location _necromancerAt;
        private bool? _eventDeckIsReshuffled;

        public void Verify(ITestRoot root)
        {
            var game = ((TestRoot) root).GetGame();
            game.Darkness.ShouldBe(_darkness);
            game.Necromancer.Location.ShouldBe(_necromancerAt);
            if (_eventDeckIsReshuffled ?? false)
                _eventDeckIsReshuffled.ShouldBe(game.Events.Count == 33);
        }

        public GameVerification Darkness(int expected)
        {
            _darkness = expected;
            return this;
        }

        public GameVerification NecromancerAt(string location)
        {
            _necromancerAt = location.ToEnum<Location>();
            return this;
        }

        public GameVerification EventDeckIsReshuffled()
        {
            _eventDeckIsReshuffled = true;
            return this;
        }
    }
}
using Shouldly;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class GameVerification : IVerifiable
    {
        private int _darkness;
        private bool? _eventDeckIsReshuffled;
        private NecromancerVerification _necromancer;

        public IVerifiable Parent => null;

        public void Verify(ITestRoot root)
        {
            var game = root.Get<Game>();
            game.Darkness.ShouldBe(_darkness);
            _necromancer?.Verify(root);
            if (_eventDeckIsReshuffled ?? false)
                _eventDeckIsReshuffled.ShouldBe(game.Events.Count == 33);
        }

        public GameVerification Darkness(int expected)
        {
            _darkness = expected;
            return this;
        }

        public NecromancerVerification Necromancer
        {
            get
            {
                _necromancer = new NecromancerVerification(this);
                return _necromancer;
            }
        }

        public GameVerification EventDeckIsReshuffled()
        {
            _eventDeckIsReshuffled = true;
            return this;
        }
    }
}
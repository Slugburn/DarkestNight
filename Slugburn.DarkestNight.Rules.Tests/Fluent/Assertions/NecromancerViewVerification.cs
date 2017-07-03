using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class NecromancerViewVerification : ChildVerification
    {
        private int _roll;
        private Location _movingTo;
        private List<string> _detected;

        public NecromancerViewVerification(IVerifiable parent) : base(parent)
        {
        }

        public override void Verify(ITestRoot root)
        {
            var player = root.Get<FakePlayer>();
            player.State.ShouldBe(PlayerState.Necromancer);

            var necromancer = player.Necromancer;
            necromancer.Roll.ShouldBe(_roll);
            necromancer.MovingTo.ShouldBe(_movingTo);
            necromancer.Detected.ShouldBe(_detected);
        }

        public NecromancerViewVerification Roll(int expected)
        {
            _roll = expected;
            return this;
        }

        public NecromancerViewVerification Detected(params string[] expected)
        {
            _detected = expected.ToList();
            return this;
        }

        public NecromancerViewVerification MovingTo(string location)
        {
            _movingTo = location.ToEnum<Location>();
            return this;
        }

    }
}
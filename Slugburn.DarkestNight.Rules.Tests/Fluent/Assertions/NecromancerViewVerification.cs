using Shouldly;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class NecromancerViewVerification : ChildVerification
    {
        private int? _roll;
        private Location? _movingTo;
        private string _detected;
        private string _description;

        public NecromancerViewVerification(IVerifiable parent) : base(parent)
        {
        }

        public override void Verify(ITestRoot root)
        {
            var game = root.Get<Game>();
            var player = root.Get<FakePlayer>();
            player.State.ShouldBe(PlayerState.Necromancer);
            var necromancer = game.Necromancer;

            var model = player.Necromancer;
            model.Roll.ShouldBe(_roll ?? necromancer.MovementRoll);
            model.Destination.ShouldBe(_movingTo ?? necromancer.Destination);
            model.DetectedHero.ShouldBe(_detected ?? necromancer.DetectedHero?.Name);
            model.Description.ShouldBe(_description);
        }

        public NecromancerViewVerification Roll(int expected)
        {
            _roll = expected;
            return this;
        }

        public NecromancerViewVerification Detected(string expected)
        {
            _detected = expected;
            return this;
        }

        public NecromancerViewVerification MovingTo(string location)
        {
            _movingTo = location.ToEnum<Location>();
            return this;
        }

        public NecromancerViewVerification Description(string expected)
        {
            _description = expected;
            return this;
        }
    }
}
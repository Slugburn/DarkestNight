using Shouldly;
using Slugburn.DarkestNight.Rules.Extensions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class NecromancerVerification : IVerifiable
    {
        private Location? _location;
        private bool _isTakingTurn;

        public void Verify(ITestRoot root)
        {
            var necromancer = root.Get<Game>().Necromancer;
            necromancer.Location.ShouldBeIfNotNull(_location, "Location");
            necromancer.IsTakingTurn.ShouldBe(_isTakingTurn);
        }

        public NecromancerVerification At(string location)
        {
            _location = location.ToEnum<Location>();
            return this;
        }

        public NecromancerVerification IsTakingTurn(bool expected = true)
        {
            _isTakingTurn = expected;
            return this;
        }
    }
}

﻿using Shouldly;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class NecromancerVerification : ChildVerification
    {
        private Location? _location;
        private bool _isTakingTurn;

        public NecromancerVerification(IVerifiable parent) : base(parent)
        {
        }

        public override void Verify(ITestRoot root)
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

using System.Linq;
using Shouldly;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class SelectedTargetModelVerification : ChildVerification
    {
        private readonly ConflictModelVerification _parent;
        private readonly string _targetName;
        private int _targetNumber;
        private int _resultNumber;
        private string _outcomeDescription;

        public SelectedTargetModelVerification(ConflictModelVerification parent, string targetName) :base(parent)
        {
            _parent = parent;
            _targetName = targetName;
        }

        public override void Verify(ITestRoot root)
        {
            var conflict = root.Get<FakePlayer>().Conflict;
            var target = conflict.SelectedTargets.Single(x => x.Name == _targetName);
            target.TargetNumber.ShouldBe(_targetNumber);
            target.ResultNumber.ShouldBe(_resultNumber);
            target.OutcomeDescription.ShouldBe(_outcomeDescription);
        }

        public SelectedTargetModelVerification Target(int expected)
        {
            _targetNumber = expected;
            return this;
        }

        public ConflictModelVerification Result(int expectedNumber, string expectedOutcomeDescription)
        {
            _resultNumber = expectedNumber;
            _outcomeDescription = expectedOutcomeDescription;
            return _parent;
        }
    }
}
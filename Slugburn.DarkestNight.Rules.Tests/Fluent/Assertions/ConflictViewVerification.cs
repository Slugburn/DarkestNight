using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class ConflictViewVerification : ChildVerification
    {
        private List<int> _roll = new List<int>();
        private string[] _tacticNames;
        private int? _targetCount;
        private string[] _targetNames;
        private bool? _win;

        public ConflictViewVerification(IVerifiable parent) : base(parent)
        {
        }

        public override void Verify(ITestRoot root)
        {
            var player = root.Get<FakePlayer>();
            var view = player.Conflict;
            view.ShouldNotBeNull();

            view.Win.ShouldBe(_win);

            if (_targetCount.HasValue)
                view.TargetCount.ShouldBe(_targetCount.Value);
            if (_targetNames != null)
                Assert.That(view.Targets.Select(x => x.Name), Is.EquivalentTo(_targetNames));
            if (_tacticNames != null)
                Assert.That(view.Tactics.Select(x => x.Name), Is.EquivalentTo(_tacticNames));
            Assert.That(view.Roll, Is.EquivalentTo(_roll));
        }

        public ConflictViewVerification HasTargets(params string[] targetNames)
        {
            _targetNames = targetNames;
            return this;
        }

        public ConflictViewVerification HasTactics(params string[] tacticNames)
        {
            _tacticNames = tacticNames;
            return this;
        }

        public ConflictViewVerification MustSelectTargets(int count)
        {
            _targetCount = count;
            return this;
        }

        public ConflictViewVerification Rolled(params int[] roll)
        {
            _roll = roll.ToList();
            return this;
        }

        public ConflictViewVerification Win(bool expected = true)
        {
            _win = expected;
            return this;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class ConflictModelVerification : ChildVerification
    {
        private List<int> _roll = new List<int>();
        private string[] _tacticNames;
        private int? _targetCount;
        private string[] _targetNames;
        private bool? _win;
        private readonly List<SelectedTargetModelVerification> _selectedTargets = new List<SelectedTargetModelVerification>();

        public ConflictModelVerification(IVerifiable parent) : base(parent)
        {
        }

        public override void Verify(ITestRoot root)
        {
            var player = root.Get<FakePlayer>();
            var view = player.Conflict;
            view.ShouldNotBeNull();

            if (_win.HasValue)
                view.SelectedTargets.Single().IsWin.ShouldBe(_win.Value);

            if (_targetCount.HasValue)
                view.TargetCount.ShouldBe(_targetCount.Value);
            if (_targetNames != null)
                Assert.That(view.Targets.Select(x => x.Name), Is.EquivalentTo(_targetNames));
            if (_tacticNames != null)
                Assert.That(view.Tactics.Select(x => x.Name), Is.EquivalentTo(_tacticNames));
            if (_roll.Any())
                Assert.That(view.Roll, Is.EquivalentTo(_roll));
            foreach (var selectedTarget in _selectedTargets)
                selectedTarget.Verify(root);
        }

        public ConflictModelVerification HasTargets(params string[] targetNames)
        {
            _targetNames = targetNames;
            return this;
        }

        public ConflictModelVerification HasTactics(params string[] tacticNames)
        {
            _tacticNames = tacticNames;
            return this;
        }

        public ConflictModelVerification MustSelectTargets(int count)
        {
            _targetCount = count;
            return this;
        }

        public ConflictModelVerification Rolled(params int[] roll)
        {
            _roll = roll.ToList();
            return this;
        }

        public ConflictModelVerification Win(bool expected = true)
        {
            _win = expected;
            return this;
        }

        public SelectedTargetModelVerification SelectedTarget(string targetName)
        {
            var verification = new SelectedTargetModelVerification(this, targetName);
            _selectedTargets.Add(verification);
            return verification;
        }
    }
}
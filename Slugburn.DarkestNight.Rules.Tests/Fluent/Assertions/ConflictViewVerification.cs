using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class ConflictViewVerification : IVerifiable
    {
        private string[] _targetNames;
        private string[] _tacticNames;

        public void Verify(ITestRoot root)
        {
            var view = root.Get<FakePlayer>();
            if (_targetNames != null)
                Assert.That(view.Conflict.Targets.Select(x=>x.Name), Is.EquivalentTo(_targetNames));
            if (_tacticNames != null)
                Assert.That(view.Conflict.Tactics.Select(x=>x.Name), Is.EquivalentTo(_tacticNames));
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
    }
}
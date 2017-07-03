using System.Linq;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class BoardLocationViewVerification : ChildVerification
    {
        private string[] _tokens;
        private int? _blightCount;

        public BoardLocationViewVerification(IVerifiable parent, string name) : base(parent)
        {
            Name = name;
        }

        public string Name { get;  }

        public override void Verify(ITestRoot root)
        {
            var board = root.Get<FakePlayer>().Board;
            var location = board.Locations.Single(loc => loc.Name == Name);
            if (_tokens != null)
                location.Tokens.ShouldBeEquivalent(_tokens);
            location.Blights.Count.ShouldBeIfNotNull(_blightCount, "BlightCount");
        }

        public BoardLocationViewVerification Token(params string[] expected)
        {
            _tokens = expected;
            return this;
        }

        public BoardLocationViewVerification BlightCount(int expected)
        {
            _blightCount = expected;
            return this;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class BoardLocationViewVerification : ChildVerification
    {
        private string[] _tokens;
        private int? _blightCount;
        private readonly List<BoardLocationBlightModelVerification> _blights 
            = new List<BoardLocationBlightModelVerification>();

        public BoardLocationViewVerification(IVerifiable parent, string name) : base(parent)
        {
            Name = name;
        }

        public string Name { get;  }

        public override void Verify(ITestRoot root)
        {
            var board = root.Get<FakePlayer>().Board;
            var location = board.Locations.Single(loc => loc.Name == Name);
            root.Set(location);
            if (_tokens != null)
                location.Tokens.ShouldBeEquivalent(_tokens);
            location.Blights.Count.ShouldBeIfNotNull(_blightCount, "BlightCount");
            foreach (var blight in _blights)
                blight.Verify(root);
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

        public BoardLocationBlightModelVerification Blight(string blightName)
        {
            var blight = new BoardLocationBlightModelVerification(this, blightName);
            _blights.Add(blight);
            return blight;
        }
    }

    public class BoardLocationBlightModelVerification : ChildVerification
    {
        private readonly string _blightName;
        private bool _isSupressed;

        public BoardLocationBlightModelVerification(IVerifiable parent, string blightName) 
            : base(parent)
        {
            _blightName = blightName;
        }

        public override void Verify(ITestRoot root)
        {
            var location = root.Get<LocationModel>();
            var blight = location.Blights.Single(x => x.Name == _blightName);
            blight.IsSupressed.ShouldBe(_isSupressed, "IsSupressed");
        }

        public BoardLocationBlightModelVerification IsSupressed(bool expected = true)
        {
            _isSupressed = expected;
            return this;
        }
    }
}
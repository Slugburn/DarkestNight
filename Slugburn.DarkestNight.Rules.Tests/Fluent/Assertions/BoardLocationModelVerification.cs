using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class BoardLocationModelVerification : ChildVerification
    {
        private int? _blightCount;
        private readonly List<BoardLocationBlightModelVerification> _blights 
            = new List<BoardLocationBlightModelVerification>();

        private bool? _isNecromancerHere;
        private string[] _effectNames;

        public BoardLocationModelVerification(IVerifiable parent, string name) : base(parent)
        {
            Name = name;
        }

        public string Name { get;  }

        public override void Verify(ITestRoot root)
        {
            var game = root.Get<Game>();
            var board = root.Get<FakePlayer>().Board;
            var location = board.Locations.Single(loc => loc.Name == Name);
            var space = game.Board[Name.ToEnum<Location>()];
            root.Set(location);
            location.Blights.Count.ShouldBeIfNotNull(_blightCount, "BlightCount");
            foreach (var blight in _blights)
                blight.Verify(root);
            _isNecromancerHere = _isNecromancerHere ?? (game.Necromancer.Location == Name.ToEnum<Location>());
            location.IsNecromancerHere.ShouldBe(_isNecromancerHere.Value, "IsNecromancerHere");

            _effectNames = _effectNames ?? space.GetEffects().Select(x => x.Name).ToArray();
            location.Effects.Select(x=>x.Name).ShouldBeEquivalent(_effectNames);
        }

        public BoardLocationModelVerification BlightCount(int expected)
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

        public BoardLocationModelVerification IsNecromancerHere(bool expected = true)
        {
            _isNecromancerHere = expected;
            return this;
        }

        public BoardLocationModelVerification Effects(params string[] expected)
        {
            _effectNames = expected;
            return this;
        }
    }
}
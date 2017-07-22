using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class BoardViewVerification : ChildVerification
    {
        private int? _darkness;
        private string _necromancerAt;
        private List<BoardLocationModelVerification> _locations;

        public BoardViewVerification(IVerifiable parent) : base(parent)
        {
            _locations = new[] {"Monastery", "Mountains", "Castle", "Village", "Ruins", "Swamp", "Forest"}
                .Select(x => new BoardLocationModelVerification(this, x)).ToList();
        }

        public override void Verify(ITestRoot root)
        {
            var game = root.Get<Game>();
            var view = root.Get<FakePlayer>().Board;
            _darkness = _darkness ?? game.Darkness;
            _necromancerAt = _necromancerAt ?? game.Necromancer.Location.ToString();
            view.Darkness.ShouldBe(_darkness.Value);
            foreach (var location in _locations)
                location.Verify(root);
        }

        public BoardViewVerification Darkness(int expected)
        {
            _darkness = expected;
            return this;
        }

        public BoardLocationModelVerification Location(string location)
        {
            return _locations.Single(x=>x.Name == location);
        }
    }
}
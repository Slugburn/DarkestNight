using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players.Models;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class PlayerHeroCommandVerification : ChildVerification
    {
        private readonly PlayerHeroVerification _parent;
        private IEnumerable<string> _expected;

        public PlayerHeroCommandVerification(IVerifiable parent) : base(parent)
        {
        }

        public override void Verify(ITestRoot root)
        {
            var hero = root.Get<Hero>();
            var view = root.Get<PlayerHero>();
            _expected = _expected ?? hero.AvailableCommands.Select(x => x.Name);
            view.Commands.Select(x=>x.Name).ShouldBeEquivalent(_expected);
        }

        public PlayerHeroCommandVerification None()
        {
            _expected = new string[0];
            return this;
        }

        public PlayerHeroCommandVerification Exactly(params string[] expected)
        {
            _expected = expected;
            return this;
        }
    }
}
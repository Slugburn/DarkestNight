using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class PlayerHeroCommandVerification : ChildVerification
    {
        private readonly PlayerHeroVerification _parent;
        private IEnumerable<string> _expected;
        private string[] _includes;
        private string[] _excludes;

        public PlayerHeroCommandVerification(IVerifiable parent) : base(parent)
        {
        }

        public override void Verify(ITestRoot root)
        {
            var hero = root.Get<Hero>();
            var view = root.Get<HeroModel>();
            var commandNames = view.Commands.Select(x => x.Name).ToList();
            _expected = _expected ?? hero.AvailableCommands.Select(x => x.Name);
            commandNames.ShouldBeEquivalent(_expected);
            if (_includes != null)
                commandNames.Intersect(_includes).ShouldBeEquivalent(_includes);
            if (_excludes != null)
            {
                var excludedCommands = commandNames.Intersect(_excludes).ToList();
                excludedCommands.ShouldBeEmpty();
            }
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

        public PlayerHeroCommandVerification Includes(params string[] expected)
        {
            _includes = expected;
            return this;
        }

        public PlayerHeroCommandVerification Excludes(params string[] expected)
        {
            _excludes = expected;
            return this;
        }
    }
}
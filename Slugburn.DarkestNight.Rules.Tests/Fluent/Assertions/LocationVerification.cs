using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class LocationVerification : IVerifiable
    {
        private readonly string _location;
        private IEnumerable<string> _blights;
        private string[] _actionExists;
        private string[] _actionDoesNotExist;
        private bool? _hasRelic;
        private int? _searchTarget;

        public LocationVerification(string location) 
        {
            _location = location;
        }

        public void Verify(ITestRoot root)
        {
            var game = root.Get<Game>();
            var space = game.Board[_location.ToEnum<Location>()];
            _blights = _blights ?? new List<string>();
            space.Blights.Select(x => x.Type.ToString()).ShouldBeEquivalent(_blights);

            space.HasRelic.ShouldBeIfNotNull(_hasRelic, "HasRelic");
            space.GetSearchTarget(null).ShouldBeIfNotNull(_searchTarget, "SearchTarget");

            if (_actionExists != null)
            {
                var actual = space.GetActions().Select(x=>x.Name).Intersect(_actionExists);
                Assert.That(actual, Is.EquivalentTo(_actionExists));
            }
            if (_actionDoesNotExist != null)
            {
                var actual = space.GetActions().Select(x => x.Name).Intersect(_actionDoesNotExist).ToList();
                Assert.That(actual, Is.Empty);
            }
        }

        public LocationVerification Blights(params string[] blightTypes)
        {
            _blights = blightTypes.ToList();
            return this;
        }

        public LocationVerification HasAction(params string[] actionNames)
        {
            _actionExists = actionNames;
            return this;
        }

        public LocationVerification DoesNotHaveAction(params string[] actionNames)
        {
            _actionDoesNotExist = actionNames;
            return this;
        }

        public LocationVerification HasRelic(bool expected)
        {
            _hasRelic = expected;
            return this;
        }

        public LocationVerification SearchTarget(int expected)
        {
            _searchTarget = expected;
            return this;
        }
    }
}
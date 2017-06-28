using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class LocationVerification : IVerifiable
    {
        private readonly string _location;
        private List<Blight> _blights;
        private string[] _actionExists;
        private string[] _actionDoesNotExist;

        public LocationVerification(string location)
        {
            _location = location;
        }

        public void Verify(ITestRoot root)
        {
            var game = root.Get<Game>();
            var space = game.Board[_location.ToEnum<Location>()];
            _blights = _blights ?? new List<Blight>();
            Assert.That(space.Blights, Is.EquivalentTo(_blights));

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

        public LocationVerification Blights(params string[] blights)
        {
            _blights = blights.Select(b=>b.ToEnum<Blight>()).ToList();
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
    }
}
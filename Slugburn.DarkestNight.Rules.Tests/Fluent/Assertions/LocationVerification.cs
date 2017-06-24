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

        public LocationVerification(string location)
        {
            _location = location;
        }

        public void Verify(ITestRoot root)
        {
            var game = root.Get<Game>();
            var space = game.Board[_location.ToEnum<Location>()];
            Assert.That(space.Blights, Is.EquivalentTo(_blights));
        }

        public LocationVerification Blights(params string[] blights)
        {
            _blights = blights.Select(b=>b.ToEnum<Blight>()).ToList();
            return this;
        }

    }
}
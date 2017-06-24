using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
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
            var game = ((TestRoot) root).GetGame();
            var space = game.Board[_location.ToEnum<Location>()];
            Assert.That(space.Blights, Is.EquivalentTo(_blights));
        }

        public LocationVerification Blights(params string[] blights)
        {
            _blights = blights.Select(b=>StringExtensions.ToEnum<Blight>(b)).ToList();
            return this;
        }

    }
}
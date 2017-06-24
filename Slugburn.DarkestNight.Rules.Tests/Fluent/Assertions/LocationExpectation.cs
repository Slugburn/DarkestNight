using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class LocationExpectation
    {
        private readonly ISpace _space;
        private readonly List<Blight> _expectedBlights = new List<Blight>();

        public LocationExpectation(ISpace space)
        {
            _space = space;
        }

        public void Verify()
        {
            Assert.That(_space.Blights, Is.EquivalentTo(_expectedBlights), "Unexpected Blights");
        }

        public LocationExpectation NoBlights()
        {
            _expectedBlights.Clear();
            return this;
        }

        public LocationExpectation Blights(params string[] blights)
        {
            _expectedBlights.AddRange(blights.Select(x=>StringExtensions.ToEnum<Blight>(x)));
            return this;
        }
    }
}
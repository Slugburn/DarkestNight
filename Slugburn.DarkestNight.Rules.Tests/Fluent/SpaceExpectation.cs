using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public class SpaceExpectation
    {
        private readonly ISpace _space;
        private readonly List<Blight> _expectedBlights = new List<Blight>();

        public SpaceExpectation(ISpace space)
        {
            _space = space;
        }

        public void Verify()
        {
            Assert.That(_space.Blights, Is.EquivalentTo(_expectedBlights), "Unexpected Blights");
        }

        public SpaceExpectation NoBlights()
        {
            _expectedBlights.Clear();
            return this;
        }

        public SpaceExpectation Blights(params string[] blights)
        {
            _expectedBlights.AddRange(blights.Select(x=>x.ToEnum<Blight>()));
            return this;
        }
    }
}
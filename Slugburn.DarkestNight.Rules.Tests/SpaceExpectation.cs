using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class SpaceExpectation
    {
        private readonly ISpace _space;
        private readonly List<BlightType> _expectedBlights = new List<BlightType>();

        public SpaceExpectation(ISpace space)
        {
            _space = space;
        }

        public void Verify()
        {
            Assert.That(_space.Blights.Select(x=>x.Type), Is.EquivalentTo(_expectedBlights), "Unexpected Blights");
        }

        public SpaceExpectation NoBlights()
        {
            _expectedBlights.Clear();
            return this;
        }

        public SpaceExpectation Blights(params BlightType[] blights)
        {
            _expectedBlights.AddRange(blights);
            return this;
        }
    }
}
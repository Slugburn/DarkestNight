using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class HeroExpectation
    {
        private readonly Hero _hero;
        private int _expectedGrace;
        private int _expectedSecrecy;
        private readonly List<Location> _invalidLocations;
        private List<Location> _specifiedLocations;
        private bool _expectedActionAvailable;

        public HeroExpectation(Hero hero)
        {
            _hero = hero;
            _expectedActionAvailable = true;
            _expectedGrace = hero.DefaultGrace;
            _expectedSecrecy = hero.DefaultSecrecy;
            _invalidLocations = new List<Location>();
            _specifiedLocations = new List<Location>();
        }

        public HeroExpectation Grace(int expected)
        {
            _expectedGrace = expected;
            return this;
        }

        public void Verify()
        {
            Assert.That(_hero.IsActionAvailable, Is.EqualTo(_expectedActionAvailable),
                _expectedActionAvailable ? "Should not have used action." : "Should have used action.");
            Assert.That(_hero.Grace, Is.EqualTo(_expectedGrace), "Unexpected Grace.");
            Assert.That(_hero.Secrecy, Is.EqualTo(_expectedSecrecy), "Unexpected Secrecy.");
            var validLocations = _hero.GetValidMovementLocations().ToList();
            var disallowedMovement = validLocations.Intersect(_invalidLocations);
            var specifiedMovement = validLocations.Intersect(_specifiedLocations);
            Assert.That(disallowedMovement, Is.EquivalentTo(Enumerable.Empty<Location>()), "Movement that should be blocked is allowed.");
            Assert.That(specifiedMovement, Is.EquivalentTo(_specifiedLocations), "Moement that should be allowed is blocked.");
        }

        public HeroExpectation Secrecy(int expected)
        {
            _expectedSecrecy = expected;
            return this;
        }

        public HeroExpectation CannotMoveTo(Location location)
        {
            _invalidLocations.Add(location);
            return this;
        }

        public HeroExpectation CanMoveTo(Location location)
        {
            _specifiedLocations.Add(location);
            return this;
        }

        public HeroExpectation HasNotUsedAction()
        {
            _expectedActionAvailable = true;
            return this;
        }

        public HeroExpectation HasUsedAction()
        {
            _expectedActionAvailable = false;
            return this;
        }
    }
}
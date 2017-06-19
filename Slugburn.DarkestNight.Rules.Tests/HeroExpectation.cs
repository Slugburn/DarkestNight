using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class HeroExpectation
    {
        private readonly Hero _hero;
        private int _expectedGrace;
        private int _expectedSecrecy;
        private readonly List<Location> _invalidLocations;
        private readonly List<Location> _specifiedLocations;
        private bool _expectedActionAvailable;
        private IEnumerable<Blight> _expectedIgnoredBlights;
        private Location _expectedLocation;

        public HeroExpectation(Hero hero)
        {
            _hero = hero;
            _expectedActionAvailable = true;
            _expectedGrace = hero.DefaultGrace;
            _expectedSecrecy = hero.DefaultSecrecy;
            _invalidLocations = new List<Location>();
            _specifiedLocations = new List<Location>();
            _expectedLocation = hero.Location;
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
            Assert.That(_hero.Location, Is.EqualTo(_expectedLocation));
            var validLocations = _hero.GetValidMovementLocations().ToList();
            var disallowedMovement = validLocations.Intersect(_invalidLocations);
            var specifiedMovement = validLocations.Intersect(_specifiedLocations);
            Assert.That(disallowedMovement, Is.EquivalentTo(Enumerable.Empty<Location>()), "Movement that should be blocked is allowed.");
            Assert.That(specifiedMovement, Is.EquivalentTo(_specifiedLocations), "Moement that should be allowed is blocked.");
            if (_expectedIgnoredBlights != null)
            {
                var ignoredBlights = AllBlights().Where(x => _hero.IsBlightIgnored(x)).ToList();
                Assert.That(ignoredBlights, Is.EquivalentTo(_expectedIgnoredBlights), "Unexpected ignored blights.");
            }
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

        public HeroExpectation LostGrace(int loss = 1)
        {
            _expectedGrace = _hero.DefaultGrace - loss;
            return this;
        }

        public HeroExpectation LostSecrecy(int loss = 1)
        {
            _expectedSecrecy = _hero.DefaultSecrecy - loss;
            return this;
        }

        public HeroExpectation IsIgnoringBlights(params Blight[] blights)
        {
            if (!blights.Any())
                blights = AllBlights();
            _expectedIgnoredBlights = blights;
            return this;
        }

        public HeroExpectation IsNotIgnoringBlights(params Blight[] blights)
        {
            _expectedIgnoredBlights = blights.Any() ? AllBlights().Except(blights) : new Blight[0];
            return this;
        }

        private static Blight[] AllBlights()
        {
            return new[]
            {
                Blight.Confusion,
                Blight.Corruption,
                Blight.Curse,
                Blight.DarkFog,
                Blight.Desecration,
                Blight.EvilPresence,
                Blight.Lich,
                Blight.Spies,
                Blight.Shades,
                Blight.Shroud,
                Blight.Skeletons,
                Blight.Taint,
                Blight.UnholyAura,
                Blight.Vampire,
                Blight.Zombies
            };
        }

        public HeroExpectation Location(Location location)
        {
            _expectedLocation = location;
            return this;
        }
    }
}
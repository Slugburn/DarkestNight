using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class HeroExpectation
    {
        private HeroEventExpectation _eventExpectation;
        private Hero _hero;
        private List<Location> _invalidLocations;
        private bool _expectedActionAvailable;
        private int _expectedAvailableMovement;
        private bool _expectedCanGainGrace;
        private int _expectedEludeDice;
        private int _expectedFightDice;
        private int _expectedGrace;
        private IEnumerable<Blight> _expectedIgnoredBlights;
        private Location _expectedLocation;
        private int _expectedSearchDice;
        private int? _expectedSecrecy;
        private int _expectedTravelSpeed;

        public HeroExpectation(Hero hero)
        {
            SetExpectations(hero);
        }

        private void SetExpectations(Hero hero)
        {
            _hero = hero;
            _expectedActionAvailable = true;
            _expectedCanGainGrace = true;
            _expectedGrace = hero.DefaultGrace;
            _expectedSecrecy = hero.DefaultSecrecy;
            _invalidLocations = new List<Location>();
            _expectedLocation = hero.Location;
            _expectedTravelSpeed = 1;
            _expectedFightDice = 1;
            _expectedEludeDice = 1;
            _expectedSearchDice = 1;
            _eventExpectation = new HeroEventExpectation(hero);
        }

        public void Verify()
        {
            Assert.That(_hero.Grace, Is.EqualTo(_expectedGrace), "Unexpected Grace.");
            if (_expectedSecrecy.HasValue)
                Assert.That(_hero.Secrecy, Is.EqualTo(_expectedSecrecy), "Unexpected Secrecy.");
            Assert.That(_hero.IsActionAvailable, Is.EqualTo(_expectedActionAvailable),
                _expectedActionAvailable ? "Should not have used action." : "Should have used action.");
            Assert.That(_hero.CanGainGrace, Is.EqualTo(_expectedCanGainGrace), "Unexpected CanGrainGrace");
            Assert.That(_hero.Location, Is.EqualTo(_expectedLocation));
            var validLocations = _hero.GetValidMovementLocations().ToList();
            var disallowedMovement = validLocations.Intersect(_invalidLocations);
            Assert.That(disallowedMovement, Is.EquivalentTo(Enumerable.Empty<Location>()), "Movement that should be blocked is allowed.");
            if (_expectedIgnoredBlights != null)
            {
                var ignoredBlights = AllBlights().Where(x => _hero.IsBlightIgnored(x)).ToList();
                Assert.That(ignoredBlights, Is.EquivalentTo(_expectedIgnoredBlights), "Unexpected ignored blights.");
            }
            Assert.That(_hero.TravelSpeed, Is.EqualTo(_expectedTravelSpeed), "Unexpected TravelSpeed.");
            Assert.That(_hero.AvailableMovement, Is.EqualTo(_expectedAvailableMovement), "Unexpected AvailableMovement.");

            if (_hero.CurrentRoll == null)
                _hero.SetRoll(RollBuilder.Create(null));
            var fightDice = _hero.CurrentRoll.GetDice(RollType.Fight, "Fight", 1).Total;
            Assert.That(fightDice, Is.EqualTo(_expectedFightDice), "Unexpected number of Fight dice.");
            var eludeDice = _hero.CurrentRoll.GetDice(RollType.Elude, "Elude", 1).Total;
            Assert.That(eludeDice, Is.EqualTo(_expectedEludeDice), "Unexpected number of Elude dice.");
            var dice = _hero.CurrentRoll.GetDice(RollType.Search, "Search", 1);
            var searchDice = dice.Total;
            Assert.That(searchDice, Is.EqualTo(_expectedSearchDice), "Unexpected number of Search dice.");
            _eventExpectation.Verify();
        }


        public HeroExpectation Grace(int expected)
        {
            _expectedGrace = expected;
            return this;
        }

        public HeroExpectation Secrecy(int? expected)
        {
            _expectedSecrecy = expected;
            return this;
        }

        public HeroExpectation CannotMoveTo(string location)
        {
            _invalidLocations.Add(location.ToEnum<Location>());
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

        public HeroExpectation CanGainGrace(bool expected = true)
        {
            _expectedCanGainGrace = expected;
            return this;
        }

        public HeroExpectation TravelSpeed(int expected)
        {
            _expectedTravelSpeed = expected;
            return this;
        }

        public HeroExpectation SearchDice(int expected)
        {
            _expectedSearchDice = expected;
            return this;
        }

        public HeroExpectation DefaultGrace(int expected)
        {
            Assert.That(_hero.DefaultGrace, Is.EqualTo(expected));
            return this;
        }

        public HeroExpectation FightDice(int expected)
        {
            _expectedFightDice = expected;
            return this;
        }

        public HeroExpectation EludeDice(int expected)
        {
            _expectedEludeDice = expected;
            return this;
        }

        public HeroExpectation AvailableMovement(int distance)
        {
            _expectedAvailableMovement = distance;
            return this;
        }

        public HeroExpectation CanUsePower(string powerName, bool expected = true)
        {
            var power = _hero.GetPower(powerName);
            Assert.That(power.IsUsable(_hero), Is.EqualTo(expected), $"{powerName} is usable.");
            return this;
        }
    }
}
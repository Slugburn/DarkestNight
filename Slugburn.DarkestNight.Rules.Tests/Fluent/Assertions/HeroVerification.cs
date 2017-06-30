using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class HeroVerification : IVerifiable
    {
        private readonly string _heroName;
        private readonly List<Location> _invalidLocations;
        private readonly List<Location> _specifiedLocations;
        private bool _expectedActionAvailable;
        private int _expectedAvailableMovement;
        private bool _expectedCanGainGrace;
        private int _expectedDiceCount;
        private int? _eludeDice;
        private int? _fightDice;
        private int? _grace;
        private int? _secrecy;
        private IEnumerable<Blight> _expectedIgnoredBlights;
        private string[] _expectedInventory;
        private Location? _location;
        private int[] _expectedRoll;
        private int? _searchDice;
        private int? _travelSpeed;
        private bool _expectedWounded;
        private int? _graceLoss;
        private int? _secrecyLoss;
        private int? _defaultGrace;
        private List<string> _expectedPowerNames;
        private int _expectedUnresolvedEvents;
        private string[] _availableActions;
        private StaticRollBonus _dieModifer;
        private bool _hasNoDieModifer;
        private Dictionary<string, bool> _powerAvailability = new Dictionary<string, bool>();
        private string[] _powerDeckContains;
        private string[] _facingEnemies;
        private bool _hasFreeAction;

        public HeroVerification(string heroName)
        {
            _heroName = heroName;
            _expectedActionAvailable = true;
            _expectedCanGainGrace = true;
            _invalidLocations = new List<Location>();
            _specifiedLocations = new List<Location>();
        }

        public HeroEventVerification CurrentEvent { get; } = new HeroEventVerification();

        private void SetExpectations(Hero hero)
        {
            _defaultGrace = _defaultGrace ?? hero.DefaultGrace;
            var validateGrace = _grace != null || _graceLoss != null;
            if (validateGrace)
                _grace = _grace ?? _defaultGrace - (_graceLoss ?? 0);
            var validateSecrecy = _secrecy != null || _secrecyLoss != null;
            if (validateSecrecy)
                _secrecy =  _secrecy ?? hero.DefaultSecrecy - (_secrecyLoss ?? 0);
            _location = _location ?? hero.Location;
        }

        public void Verify(ITestRoot root)
        {
            var hero = ((TestRoot)root).GetHero(_heroName);
            SetExpectations(hero);
            hero.SavedByGrace.ShouldBe(_expectedWounded);
            hero.DefaultGrace.ShouldBe(_defaultGrace ?? 0);
            if (_grace.HasValue)
                Assert.That(hero.Grace, Is.EqualTo(_grace), $"Unexpected Grace for {_heroName}.");
            if (_secrecy.HasValue)
                Assert.That(hero.Secrecy, Is.EqualTo(_secrecy), "Unexpected Secrecy.");
            Assert.That(hero.IsActionAvailable, Is.EqualTo(_expectedActionAvailable),
                _expectedActionAvailable ? "Should not have used action." : "Should have used action.");
            Assert.That(hero.CanGainGrace, Is.EqualTo(_expectedCanGainGrace), "Unexpected CanGainGrace");
            Assert.That(hero.Location, Is.EqualTo(_location));
            var validLocations = hero.GetValidMovementLocations().ToList();
            var disallowedMovement = validLocations.Intersect(_invalidLocations);
            var specifiedMovement = validLocations.Intersect(_specifiedLocations);
            Assert.That(disallowedMovement, Is.EquivalentTo(Enumerable.Empty<Location>()), "Movement that should be blocked is allowed.");
            Assert.That(specifiedMovement, Is.EquivalentTo(_specifiedLocations), "Moement that should be allowed is blocked.");
            if (_expectedIgnoredBlights != null)
            {
                var ignoredBlights = AllBlights().Where(x => hero.IsBlightIgnored(x)).ToList();
                Assert.That(ignoredBlights, Is.EquivalentTo(_expectedIgnoredBlights), "Unexpected ignored blights.");
            }
            if (_travelSpeed != null)
                Assert.That(hero.TravelSpeed, Is.EqualTo(_travelSpeed), "Unexpected TravelSpeed.");
            Assert.That(hero.AvailableMovement, Is.EqualTo(_expectedAvailableMovement), "Unexpected AvailableMovement.");

            if (hero.CurrentRoll == null)
                hero.SetRoll(RollBuilder.Create(null));
            var fightDice = hero.CurrentRoll.GetDice(RollType.Fight, "Fight", 1).Total;
            if (_fightDice != null)
                Assert.That(fightDice, Is.EqualTo(_fightDice), "Unexpected number of Fight dice.");
            var eludeDice = hero.CurrentRoll.GetDice(RollType.Elude, "Elude", 1).Total;
            if (_eludeDice != null)
                Assert.That(eludeDice, Is.EqualTo(_eludeDice), "Unexpected number of Elude dice.");
            var dice = hero.CurrentRoll.GetDice(RollType.Search, "Search", 1);
            var searchDice = dice.Total;
            if (_searchDice != null)
                Assert.That(searchDice, Is.EqualTo(_searchDice), "Unexpected number of Search dice.");
            if (_expectedDiceCount > 0)
                hero.CurrentRoll.AdjustedRoll.Count.ShouldBe(_expectedDiceCount, "Unexpected number of dice rolled.");
            if (_expectedRoll != null)
            {
                Assert.That(hero.CurrentRoll, Is.Not.Null, "No roll was made");
                Assert.That(hero.CurrentRoll.AdjustedRoll, Is.EquivalentTo(_expectedRoll));
            }
            hero.HasFreeAction.ShouldBe(_hasFreeAction);
            if (_expectedInventory != null)
                hero.GetInventory().Select(x=>x.Name).ShouldBeEquivalent(_expectedInventory);

            if (_expectedPowerNames != null)
                hero.Powers.Select(x => x.Name).OrderBy(x => x).ShouldBe(_expectedPowerNames);
            var unresolvedEvents = hero.EventQueue.Count + (hero.CurrentEvent != null ? 1 : 0);
            unresolvedEvents.ShouldBe(_expectedUnresolvedEvents);
            CurrentEvent.Verify(root);

            VerifyActions(hero);

            VerifyDice(hero);

            if (_powerDeckContains != null)
                Assert.That(hero.PowerDeck, Is.EquivalentTo(_powerDeckContains));
            if (_facingEnemies != null)
                hero.Enemies.Select(x => x.Name).ShouldBeEquivalent(_facingEnemies);;
        }

        private void VerifyActions(Hero hero)
        {
            if (hero.AvailableActions == null)
                hero.UpdateAvailableActions();
            if (_availableActions != null)
                Assert.That(hero.AvailableActions, Is.EquivalentTo(_availableActions));
            foreach (var kvp in _powerAvailability)
            {
                var actionName = kvp.Key;
                var expected = kvp.Value;
                var actual = hero.AvailableActions.Contains(actionName);
                var expectedDescription = expected ? "" : "not ";
                actual.ShouldBe(expected, $"Expected action '{actionName}' to {expectedDescription}be available");
            }
        }

        private void VerifyDice(Hero hero)
        {
            if (_hasNoDieModifer)
                hero.GetRollModifiers().Count().ShouldBe(0);
            if (_dieModifer != null)
            {
                var match = hero.GetRollModifiers().SingleOrDefault(x => x.Name == _dieModifer.Name);
                if (match == null)
                    Assert.Fail("No matching die modifier was found.");
                var mod = match.GetModifier(hero, _dieModifer.RollType);
                Assert.That(mod, Is.EqualTo(_dieModifer.DieCount));
            }
        }


        public HeroVerification Grace(int? expected)
        {
            _grace = expected;
            return this;
        }

        public HeroVerification Secrecy(int? expected)
        {
            _secrecy = expected;
            return this;
        }

        public HeroVerification CannotMoveTo(string location)
        {
            _invalidLocations.Add(location.ToEnum<Location>());
            return this;
        }

        public HeroVerification CanMoveTo(params string[] location)
        {
            _specifiedLocations.AddRange(location.Select(l => l.ToEnum<Location>()));
            return this;
        }

        public HeroVerification HasNotUsedAction()
        {
            _expectedActionAvailable = true;
            return this;
        }

        public HeroVerification HasUsedAction()
        {
            _expectedActionAvailable = false;
            return this;
        }

        public HeroVerification LostGrace(int loss = 1)
        {
            _graceLoss = loss;
            return this;
        }

        public HeroVerification LostSecrecy(int loss = 1)
        {
            _secrecyLoss = loss;
            return this;
        }

        public HeroVerification IsIgnoringBlights(params Blight[] blights)
        {
            if (!blights.Any())
                blights = AllBlights();
            _expectedIgnoredBlights = blights;
            return this;
        }

        public HeroVerification IsNotIgnoringBlights(params Blight[] blights)
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

        public HeroVerification Location(string location)
        {
            _location = location.ToEnum<Location>();
            return this;
        }

        public HeroVerification CanGainGrace(bool expected = true)
        {
            _expectedCanGainGrace = expected;
            return this;
        }

        public HeroVerification TravelSpeed(int expected)
        {
            _travelSpeed = expected;
            return this;
        }

        public HeroVerification SearchDice(int expected)
        {
            _searchDice = expected;
            return this;
        }

        public HeroVerification DefaultGrace(int expected)
        {
            _defaultGrace = expected;
            return this;
        }

        public HeroVerification FightDice(int expected)
        {
            _fightDice = expected;
            return this;
        }

        public HeroVerification EludeDice(int expected)
        {
            _eludeDice = expected;
            return this;
        }

        public HeroVerification AvailableMovement(int distance)
        {
            _expectedAvailableMovement = distance;
            return this;
        }

        public HeroVerification Rolled(params int[] expected)
        {
            _expectedRoll = expected;
            return this;
        }

        public HeroVerification RolledNumberOfDice(int expectedDice)
        {
            _expectedDiceCount = expectedDice;
            return this;
        }

        public HeroVerification HasFreeAction(bool expected = true)
        {
            _hasFreeAction = expected;
            return this;
        }

        public HeroVerification HasItems(params string[] items)
        {
            _expectedInventory = items;
            return this;
        }

        public HeroVerification WasWounded(bool expected = true)
        {
            _expectedWounded = expected;
            _graceLoss = expected ? 1 : 0;
            return this;
        }

        public HeroVerification HasPowers(params string[] powerNames)
        {
            _expectedPowerNames = powerNames.OrderBy(x => x).ToList();
            return this;
        }

        public HeroVerification HasUnresolvedEvents(int expected)
        {
            _expectedUnresolvedEvents = expected;
            return this;
        }

        public HeroVerification HasAvailableActions(params string[] expected)
        {
            _availableActions = expected;
            return this;
        }

        public HeroVerification HasDieModifier(string name, RollType rollType, int mod)
        {
            _dieModifer = new StaticRollBonus() {Name = name, RollType = rollType, DieCount = mod};
            return this;
        }

        public HeroVerification HasNoDieModifier()
        {
            _hasNoDieModifer = true;
            return this;
        }

        public HeroVerification CanTakeAction(string actionName, bool isAvailable = true)
        {
            _powerAvailability[actionName] = isAvailable;
            return this;
        }

        public HeroVerification PowerDeckContains(params string[] powerName)
        {
            _powerDeckContains = powerName;
            return this;
        }

        public HeroVerification IsFacingEnemies(params string[] expected)
        {
            _facingEnemies = expected;
            return this;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class HeroVerification : IVerifiable
    {
        private readonly string _heroName;
        private readonly List<Location> _invalidLocations;
        private readonly List<Location> _specifiedLocations;
        private bool? _isActionAvailable;
        private int _expectedAvailableMovement;
        private bool _expectedCanGainGrace;
        private int _expectedDiceCount;
        private int? _eludeDice;
        private int? _fightDice;
        private int? _grace;
        private int? _secrecy;
        private bool? _isIgnoringBlights;
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
        private string _dieModifer;
        private bool _hasNoDieModifer;
        private Dictionary<string, bool> _powerAvailability = new Dictionary<string, bool>();
        private string[] _powerDeckContains;
        private string[] _facingEnemies;
        private bool _hasFreeAction;
        private bool? _isTakingTurn;

        public IVerifiable Parent => null;

        public HeroVerification(string heroName) 
        {
            _heroName = heroName;
            _expectedCanGainGrace = true;
            _invalidLocations = new List<Location>();
            _specifiedLocations = new List<Location>();
            CurrentEvent = new HeroEventVerification(this);
        }

        public HeroEventVerification CurrentEvent { get; } 

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
            var game = root.Get<Game>();
            var hero = ((TestRoot)root).GetHero(_heroName);
            SetExpectations(hero);
            hero.IsTakingTurn.ShouldBeIfNotNull(_isTakingTurn, "IsTakingTurn");
            hero.SavedByGrace.ShouldBe(_expectedWounded);
            hero.DefaultGrace.ShouldBe(_defaultGrace ?? 0);
            if (_grace.HasValue)
                Assert.That(hero.Grace, Is.EqualTo(_grace), $"Unexpected Grace for {hero.Name}.");
            if (_secrecy.HasValue)
                Assert.That(hero.Secrecy, Is.EqualTo(_secrecy), "Unexpected Secrecy.");
            hero.IsActionAvailable.ShouldBeIfNotNull(_isActionAvailable, "IsActionAvailable");
            Assert.That(hero.CanGainGrace(), Is.EqualTo(_expectedCanGainGrace), "Unexpected CanGainGrace");
            Assert.That(hero.Location, Is.EqualTo(_location));
            var validLocations = hero.GetValidMovementLocations().ToList();
            var disallowedMovement = validLocations.Intersect(_invalidLocations);
            var specifiedMovement = validLocations.Intersect(_specifiedLocations);
            Assert.That(disallowedMovement, Is.EquivalentTo(Enumerable.Empty<Location>()), "Movement that should be blocked is allowed.");
            Assert.That(specifiedMovement, Is.EquivalentTo(_specifiedLocations), "Movement that should be allowed is blocked.");
            if (_isIgnoringBlights != null)
            {
                var blights = game.GetBlights().ToList();
                if (_isIgnoringBlights.Value)
                {
                    var activeBlights = blights.Where(b => !game.IsBlightSupressed(b, hero)).ToList();
                    activeBlights.ShouldBeEmpty();
                }
                else
                {
                    var ignoredBlights = blights.Where(b => game.IsBlightSupressed(b, hero)).ToList();
                    ignoredBlights.ShouldBeEmpty();
                }
            }
            if (_travelSpeed != null)
                Assert.That(hero.TravelSpeed, Is.EqualTo(_travelSpeed), "Unexpected TravelSpeed.");
            Assert.That(hero.AvailableMovement, Is.EqualTo(_expectedAvailableMovement), "Unexpected AvailableMovement.");

            if (hero.CurrentRoll == null)
                hero.SetRoll(RollBuilder.Create(null));
            RollState rollState = hero.CurrentRoll;
            var fightDice = rollState.Hero.CreateModifierSummary(ModifierType.FightDice, "Fight", 1).Total;
            if (_fightDice != null)
                Assert.That(fightDice, Is.EqualTo(_fightDice), "Unexpected number of Fight dice.");
            RollState rollState1 = hero.CurrentRoll;
            var eludeDice = rollState1.Hero.CreateModifierSummary(ModifierType.EludeDice, "Elude", 1).Total;
            if (_eludeDice != null)
                Assert.That(eludeDice, Is.EqualTo(_eludeDice), "Unexpected number of Elude dice.");
            RollState rollState2 = hero.CurrentRoll;
            var dice = rollState2.Hero.CreateModifierSummary(ModifierType.SearchDice, "Search", 1);
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
                hero.Enemies.Select(x => x.Name).ShouldBeEquivalent(_facingEnemies);
        }

        private void VerifyActions(Hero hero)
        {
            if (hero.AvailableCommands == null)
                hero.UpdateAvailableCommands();
            if (_availableActions != null)
                hero.AvailableCommands.Select(x=>x.Name).ShouldBeEquivalent(_availableActions);
            foreach (var kvp in _powerAvailability)
            {
                var actionName = kvp.Key;
                var expected = kvp.Value;
                var actual = hero.AvailableCommands.Select(x=>x.Name).Contains(actionName);
                var expectedDescription = expected ? "" : "not ";
                actual.ShouldBe(expected, $"Expected action '{actionName}' to {expectedDescription}be available");
            }
        }

        private void VerifyDice(Hero hero)
        {
            if (_hasNoDieModifer)
                hero.GetModifiers().Count().ShouldBe(0);
            if (_dieModifer != null)
            {
                var match = hero.GetModifiers().SingleOrDefault(x => x.Name == _dieModifer);
                if (match == null)
                    Assert.Fail("No matching die modifier was found.");
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
            _isActionAvailable = true;
            return this;
        }

        public HeroVerification HasUsedAction()
        {
            _isActionAvailable = false;
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

        public HeroVerification IsIgnoringBlights(bool expected = true)
        {
            _isIgnoringBlights = expected;
            return this;
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

        public HeroVerification HasDieModifier(string name)
        {
            _dieModifer = name;
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

        public HeroVerification IsTakingTurn(bool expected = true)
        {
            _isTakingTurn = expected;
            return this;
        }
    }
}

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
        private readonly List<Location> _invalidLocations;
        private readonly List<Location> _specifiedLocations;
        private bool _expectedActionAvailable;
        private int _expectedAvailableMovement;
        private bool _expectedCanGainGrace;
        private int _expectedDiceCount;
        private int _expectedEludeDice;
        private int _expectedFightDice;
        private int _expectedFreeActions;
        private int? _expectedGrace;
        private int? _expectedSecrecy;
        private IEnumerable<Blight> _expectedIgnoredBlights;
        private string[] _expectedInventory = new string[0];
        private Location? _expectedLocation;
        private int[] _expectedRoll;
        private int _expectedSearchDice;
        private int _expectedTravelSpeed;
        private bool _expectedWounded;
        private int? _expectedGraceLoss;
        private int? _expectedSecrecyLoss;
        private int? _expectedDefaultGrace;
        private List<string> _expectedPowerNames;
        private int _expectedUnresolvedEvents;
        private string[] _availableActions;
        private StaticRollBonus _dieModifer;
        private bool _hasNoDieModifer;


        public HeroVerification()
        {
            _expectedActionAvailable = true;
            _expectedCanGainGrace = true;
            _invalidLocations = new List<Location>();
            _specifiedLocations = new List<Location>();
            _expectedTravelSpeed = 1;
            _expectedFightDice = 1;
            _expectedEludeDice = 1;
            _expectedSearchDice = 1;
        }

        public HeroEventVerification CurrentEvent { get; } = new HeroEventVerification();

        private void SetExpectations(Hero hero)
        {
            _expectedDefaultGrace = _expectedDefaultGrace ?? hero.DefaultGrace;
            _expectedGrace = _expectedGrace ?? _expectedDefaultGrace - (_expectedGraceLoss ?? 0);
            var validateSecrecy = _expectedSecrecy != null || _expectedSecrecyLoss != null;
            if (validateSecrecy)
                _expectedSecrecy =  _expectedSecrecy ?? hero.DefaultSecrecy - (_expectedSecrecyLoss ?? 0);
            _expectedLocation = _expectedLocation ?? hero.Location;
        }

        public void Verify(ITestRoot root)
        {
            var hero = root.Get<Game>().ActingHero;
            SetExpectations(hero);
            hero.SavedByGrace.ShouldBe(_expectedWounded);
            hero.DefaultGrace.ShouldBe(_expectedDefaultGrace ?? 0);
            Assert.That(hero.Grace, Is.EqualTo(_expectedGrace), "Unexpected Grace.");
            if (_expectedSecrecy.HasValue)
                Assert.That(hero.Secrecy, Is.EqualTo(_expectedSecrecy), "Unexpected Secrecy.");
            Assert.That(hero.IsActionAvailable, Is.EqualTo(_expectedActionAvailable),
                _expectedActionAvailable ? "Should not have used action." : "Should have used action.");
            Assert.That(hero.CanGainGrace, Is.EqualTo(_expectedCanGainGrace), "Unexpected CanGrainGrace");
            Assert.That(hero.Location, Is.EqualTo(_expectedLocation));
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
            Assert.That(hero.TravelSpeed, Is.EqualTo(_expectedTravelSpeed), "Unexpected TravelSpeed.");
            Assert.That(hero.AvailableMovement, Is.EqualTo(_expectedAvailableMovement), "Unexpected AvailableMovement.");

            if (hero.CurrentRoll == null)
                hero.SetRoll(RollBuilder.Create(null));
            var fightDice = hero.CurrentRoll.GetDice(RollType.Fight, "Fight", 1).Total;
            Assert.That(fightDice, Is.EqualTo(_expectedFightDice), "Unexpected number of Fight dice.");
            var eludeDice = hero.CurrentRoll.GetDice(RollType.Elude, "Elude", 1).Total;
            Assert.That(eludeDice, Is.EqualTo(_expectedEludeDice), "Unexpected number of Elude dice.");
            var dice = hero.CurrentRoll.GetDice(RollType.Search, "Search", 1);
            var searchDice = dice.Total;
            Assert.That(searchDice, Is.EqualTo(_expectedSearchDice), "Unexpected number of Search dice.");
            if (_expectedDiceCount > 0)
                Assert.That(hero.CurrentRoll.AdjustedRoll.Count, Is.EqualTo(_expectedDiceCount));
            if (_expectedRoll != null)
            {
                Assert.That(hero.CurrentRoll, Is.Not.Null, "No roll was made");
                Assert.That(hero.CurrentRoll.AdjustedRoll, Is.EquivalentTo(_expectedRoll));
            }
            Assert.That(hero.FreeActions, Is.EqualTo(_expectedFreeActions));
            hero.Inventory.ShouldBe(_expectedInventory);

            if (_expectedPowerNames != null)
                hero.Powers.Select(x => x.Name).OrderBy(x => x).ShouldBe(_expectedPowerNames);
            if (_availableActions != null)
                Assert.That(hero.AvailableActions, Is.EquivalentTo(_availableActions));
            var unresolvedEvents = hero.EventQueue.Count + (hero.CurrentEvent != null ? 1 : 0);
            unresolvedEvents.ShouldBe(_expectedUnresolvedEvents);
            CurrentEvent.Verify(root);

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


        public HeroVerification Grace(int expected)
        {
            _expectedGrace = expected;
            return this;
        }

        public HeroVerification Secrecy(int? expected)
        {
            _expectedSecrecy = expected;
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
            _expectedGraceLoss = loss;
            return this;
        }

        public HeroVerification LostSecrecy(int loss = 1)
        {
            _expectedSecrecyLoss = loss;
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
            _expectedLocation = location.ToEnum<Location>();
            return this;
        }

        public HeroVerification CanGainGrace(bool expected = true)
        {
            _expectedCanGainGrace = expected;
            return this;
        }

        public HeroVerification TravelSpeed(int expected)
        {
            _expectedTravelSpeed = expected;
            return this;
        }

        public HeroVerification SearchDice(int expected)
        {
            _expectedSearchDice = expected;
            return this;
        }

        public HeroVerification DefaultGrace(int expected)
        {
            _expectedDefaultGrace = expected;
            return this;
        }

        public HeroVerification FightDice(int expected)
        {
            _expectedFightDice = expected;
            return this;
        }

        public HeroVerification EludeDice(int expected)
        {
            _expectedEludeDice = expected;
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

        public HeroVerification HasFreeAction()
        {
            _expectedFreeActions = 1;
            return this;
        }

        public HeroVerification HasItems(params string[] items)
        {
            _expectedInventory = items;
            return this;
        }

        public HeroVerification WasWounded(bool expected = true)
        {
            _expectedWounded = true;
            _expectedGraceLoss = 1;
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
    }
}

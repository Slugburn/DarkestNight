using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class PlayerVerification : IVerifiable
    {
        private EventViewVerification _eventView;
        private List<string> _locations;
        private List<string> _powerNames;
        private BlightSelectionViewVerification _blightSelectionView;
        private ConflictModelVerification _conflictModel;
        private NecromancerViewVerification _necromancerView;
        private string[] _heroNames;
        private SearchViewVerification _searchView;
        private PrayerViewVerification _prayerView;
        private readonly Dictionary<string, PlayerHeroVerification>  _heroViews = new Dictionary<string, PlayerHeroVerification>();


        public PlayerVerification()
        {
            BoardView = new BoardViewVerification(this);
        }

        public void Verify(ITestRoot root)
        {
            var game = root.Get<Game>();
            var player = root.Get<FakePlayer>();
            if (_locations != null)
                Assert.That(player.ValidLocations, Is.EquivalentTo(_locations));
            if (_powerNames != null)
                Assert.That(player.Powers.Select(x=>x.Name), Is.EquivalentTo(_powerNames));
            if (_heroNames != null)
                Assert.That(player.HeroSelection.Heroes, Is.EquivalentTo(_heroNames));

            var playerHeroVerifications = game.Heroes.Select(hero => hero.Name).Select(name => _heroViews.ContainsKey(name) ? _heroViews[name] : new PlayerHeroVerification(this, name));
            foreach (var verification in playerHeroVerifications)
                verification.Verify(root);

            BoardView.Verify(root);
            _conflictModel?.Verify(root);
            _eventView?.Verify(root);
            _blightSelectionView?.Verify(root);
            _necromancerView?.Verify(root);
            _searchView?.Verify(root);
            _prayerView?.Verify(root);
        }

        public EventViewVerification EventView
        {
            get
            {
                _eventView = new EventViewVerification(this);
                return _eventView;
            }
        }

        public BlightSelectionViewVerification BlightSelectionView
        {
            get
            {
                _blightSelectionView = new BlightSelectionViewVerification(this);
                return _blightSelectionView;
            }
        }

        public ConflictModelVerification ConflictModel
        {
            get
            {
                _conflictModel = new ConflictModelVerification(this);
                return _conflictModel;
            }
        }

        public NecromancerViewVerification NecromancerView
        {
            get
            {
                _necromancerView = new NecromancerViewVerification(this);
                return _necromancerView;
            }
        }

        public SearchViewVerification SearchView
        {
            get
            {
                _searchView = new SearchViewVerification(this);
                return _searchView;
            }
        }

        public PrayerViewVerification PrayerView
        {
            get
            {
                _prayerView = new PrayerViewVerification(this);
                return _prayerView;
            } 
        }

        public BoardViewVerification BoardView { get; }

        public PlayerVerification LocationSelectionView(params string[] locations)
        {
            _locations = locations.OrderBy(x=>x).ToList();
            return this;
        }

        public PlayerVerification PowerSelectionView(params string[] powerNames)
        {
            _powerNames = powerNames.OrderBy(x=>x).ToList();
            return this;
        }

        public PlayerVerification HeroSelectionView(params string[] heroNames)
        {
            _heroNames = heroNames;
            return this;
        }

        public PlayerHeroVerification Hero(string heroName = "Joe")
        {
            var verification = new PlayerHeroVerification(this, heroName);
            _heroViews.Add(heroName, verification);
            return verification;
        }
    }
}
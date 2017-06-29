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
        private ConflictViewVerification _conflictView;
        private NecromancerViewVerification _necromancerView;
        private string[] _heroNames;
        private SearchResultSelectionVerification _searchResultSelection;

        public void Verify(ITestRoot root)
        {
            var player = root.Get<FakePlayer>();
            if (_locations != null)
                Assert.That(player.ValidLocations, Is.EquivalentTo(_locations));
            if (_powerNames != null)
                Assert.That(player.Powers.Select(x=>x.Name), Is.EquivalentTo(_powerNames));
            if (_heroNames != null)
                Assert.That(player.HeroSelection.Heroes.Select(x=>x.Name), Is.EquivalentTo(_heroNames));

            _conflictView?.Verify(root);
            _eventView?.Verify(root);
            _blightSelectionView?.Verify(root);
            _necromancerView?.Verify(root);
            _searchResultSelection?.Verify(root);
        }

        public EventViewVerification EventView
        {
            get
            {
                _eventView = new EventViewVerification();
                return _eventView;
            }
        }

        public BlightSelectionViewVerification BlightSelectionView
        {
            get
            {
                _blightSelectionView = new BlightSelectionViewVerification();
                return _blightSelectionView;
            }
        }

        public ConflictViewVerification ConflictView
        {
            get
            {
                _conflictView = new ConflictViewVerification();
                return _conflictView;
            }
        }

        public NecromancerViewVerification NecromancerView
        {
            get
            {
                _necromancerView = new NecromancerViewVerification();
                return _necromancerView;
            }
        }

        public SearchResultSelectionVerification SearchView
        {
            get
            {
                _searchResultSelection = new SearchResultSelectionVerification();
                return _searchResultSelection;
            }
        }

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
    }
}
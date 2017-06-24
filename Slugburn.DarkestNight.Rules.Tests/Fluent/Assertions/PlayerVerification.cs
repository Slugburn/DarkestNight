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

        public void Verify(ITestRoot root)
        {
            var player = root.Get<FakePlayer>();
            if (_locations != null)
                Assert.That(player.ValidLocations, Is.EquivalentTo(_locations));
            if (_powerNames != null)
                Assert.That(player.Powers.Select(x=>x.Name), Is.EquivalentTo(_powerNames));

            _eventView?.Verify(root);
            _blightSelectionView?.Verify(root);
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
    }
}
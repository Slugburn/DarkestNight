using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class BlightSelectionViewVerification : ChildVerification
    {
        private readonly List<BlightLocationVerification> _locations = new List<BlightLocationVerification>();
        private readonly List<PlayerBlight> _playerBlights = new List<PlayerBlight>();
        private int _max = 1;

        public BlightSelectionViewVerification(IVerifiable parent) : base(parent)
        {
        }

        public override void Verify(ITestRoot root)
        {
            var view = root.Get<FakePlayer>().BlightSelection;
            var expected = _playerBlights.Select(ToDescription);
            var actual = view.Blights.Select(ToDescription);
            Assert.That(actual, Is.EquivalentTo(expected));

            view.Max.ShouldBe(_max);
        }

        private static string ToDescription(PlayerBlight x)
        {
            return $"{x.Location}-{x.BlightType}";
        }

        public BlightLocationVerification Location(string location)
        {
            var verification = new BlightLocationVerification(this, location.ToEnum<Location>());
            _locations.Add(verification);
            return verification;
        }

        public BlightSelectionViewVerification Max(int max)
        {
            _max = max;
            return this;
        }

        public class BlightLocationVerification : ChildVerification
        {
            private readonly Location _location;

            public BlightLocationVerification(BlightSelectionViewVerification parent, Location location) :base(parent)
            {
                _location = location;
            }

            public override void Verify(ITestRoot root)
            {
            }

            public BlightSelectionViewVerification WithBlights(params string[] blights)
            {
                var playerBlights = blights.Select(x => x.ToEnum<BlightType>())
                    .Select(blight => new PlayerBlight {Location = _location.ToString(), BlightType = blight.ToString()});
                var parent = (BlightSelectionViewVerification)Parent;
                parent._playerBlights.AddRange(playerBlights);
                return parent;
            }
        }
    }
}
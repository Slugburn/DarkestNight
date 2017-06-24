using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Slugburn.DarkestNight.Rules.Players.Models;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class PlayerBlightExpectation
    {
        private readonly ICollection<PlayerBlight> _blights;
        private readonly List<string> _expected = new List<string>();

        public PlayerBlightExpectation(ICollection<PlayerBlight> blights)
        {
            blights.ShouldNotBeNull("Player.Blights has not been set.");
            _blights = blights;
        }

        public PlayerBlightExpectation Location(string location, params string[] blights)
        {
            _expected.AddRange(blights.Select(b => $"{location}:{b}"));
            return this;
        }

        internal void Verify()
        {
            var actual = _blights.Select(x => $"{x.Location}:{x.Blight}").OrderBy(x => x);
            actual.ShouldBe(_expected.OrderBy(x => x));
        }
    }
}
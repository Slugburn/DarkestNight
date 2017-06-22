using System.Linq;
using NUnit.Framework;
using Shouldly;
using Slugburn.DarkestNight.Rules.Players.Models;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public class PlayerEventExpectation
    {
        private readonly PlayerEvent _playerEvent;

        public PlayerEventExpectation(PlayerEvent playerEvent)
        {
            _playerEvent = playerEvent;
        }

        public PlayerEventExpectation HasBody(string title, int fate, string text)
        {
            var e = _playerEvent;
            e.Title.ShouldBe(title);
            e.Text.ShouldBe(text);
            e.Fate.ShouldBe(fate);
            return this;
        }

        public PlayerEventExpectation HasOptions(params string[] options)
        {
            _playerEvent.Options.Select(x=>x.Text).ShouldBe(options);
            return this;
        }

        public PlayerEventExpectation ActiveRow(int min, int max, string text, string subText = null)
        {
            var e = _playerEvent;
            e.Rows.ShouldNotBeNull();
            var row = e.Rows.SingleOrDefault(r => r.Min == min && r.Max == max);
            if (row == null)
                Assert.Fail();
            if (!row.IsActive)
            {
                var actual = e.Rows.SingleOrDefault(x => x.IsActive);
                if (actual == null)
                    Assert.Fail("No row is active");
                else
                    Assert.Fail($"{actual.Text} is active");
            }
            row.IsActive.ShouldBeTrue();
            row.Text.ShouldBe(text);
            row.SubText.ShouldBe(subText);
            return this;
        }

    }
}
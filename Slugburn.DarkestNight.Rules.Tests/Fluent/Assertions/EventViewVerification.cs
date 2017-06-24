using System.Linq;
using NUnit.Framework;
using Shouldly;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class EventViewVerification : IVerifiable
    {
        private string[] _options;
        private string _activeRowText;
        private string _activeRowSubText;
        private string _title;
        private int _fate;
        private string _text;

        public void Verify(ITestRoot root)
        {
            var player = root.Get<FakePlayer>();
            var view = player.Event;
            if (_title != null)
            {
                view.Title.ShouldBe(_title);
                view.Fate.ShouldBe(_fate);
                view.Text.ShouldBe(_text);
            }
            if (_options != null)
                view.Options.Select(x => x.Text).ShouldBe(_options);
            VerifyActiveRow(view);
        }

        private void VerifyActiveRow(PlayerEvent view)
        {
            var text = _activeRowText;
            var subText = _activeRowSubText;
            if (_activeRowText == null) return;
            view.Rows.ShouldNotBeNull();
            var row = view.Rows.SingleOrDefault(r => r.Text == text);
            if (row == null)
                Assert.Fail($"Row for \"{text}\" was not found.");
            if (row.IsActive)
            {
                row.IsActive.ShouldBeTrue();
                row.Text.ShouldBe(text);
                row.SubText.ShouldBe(subText);
                return;
            }
            var actual = view.Rows.SingleOrDefault(x => x.IsActive);
            Assert.Fail(actual == null ? "No row is active" : $"\"{actual.Text}\" is the active row.");
        }

        public EventViewVerification HasOptions(params string[] options)
        {
            _options = options;
            return this;
        }

        public EventViewVerification ActiveRow(string text, string subText = null)
        {
            _activeRowText = text;
            _activeRowSubText = subText;
            return this;
        }

        public EventViewVerification HasBody(string title, int fate, string text)
        {
            _title = title;
            _fate = fate;
            _text = text;
            return this;
        }
    }
}
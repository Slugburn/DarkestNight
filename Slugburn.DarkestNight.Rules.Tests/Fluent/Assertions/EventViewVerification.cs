using System.Linq;
using NUnit.Framework;
using Shouldly;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class EventViewVerification : ChildVerification
    {
        private string[] _options;
        private string _activeRowText;
        private string _activeRowSubText;
        private string _title;
        private int? _fate;
        private string _text;
        private bool _isVisible = true;

        public EventViewVerification(IVerifiable parent) : base(parent)
        {
        }

        public override void Verify(ITestRoot root)
        {
            var player = root.Get<FakePlayer>();
            var view = player.Event;
            if (_isVisible)
                view.ShouldNotBeNull();
            else
                view.ShouldBeNull();
            if (_title != null)
            {
                view.Title.ShouldBe(_title);
                view.Fate.ShouldBeIfNotNull(_fate, "Fate");
                view.Text.ShouldBeIfNotNull(_text, "Text");
            }
            if (_options != null)
                view.Options.Select(x => x.Text).ShouldBe(_options);
            VerifyActiveRow(view);
        }

        public EventViewVerification IsVisible(bool expected = true)
        {
            _isVisible = expected;
            return this;
        }

        private void VerifyActiveRow(EventModel view)
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

        public EventViewVerification HasTitle(string expected)
        {
            _title = expected;
            return this;
        }

        public EventViewVerification HasBody(string title, int fate, string text)
        {
            _title = title;
            _fate = fate;
            _text = text;
            return this;
        }

        public EventViewVerification HasFate(int expected)
        {
            _fate = expected;
            return this;
        }

        public EventViewVerification HasText(string expected)
        {
            _text = expected;
            return this;
        }
    }
}
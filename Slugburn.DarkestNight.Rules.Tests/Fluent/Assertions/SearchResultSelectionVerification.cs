using NUnit.Framework;
using Shouldly;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class SearchResultSelectionVerification : IVerifiable
    {
        private string[] _results;
        private int[] _roll;

        public void Verify(ITestRoot root)
        {
            var view = root.Get<FakePlayer>().Search;
            view.ShouldNotBeNull("Search result selection is null");
            if (_results!= null)
                Assert.That(view.SearchResults, Is.EquivalentTo(_results));
            if (_roll != null)
                Assert.That(view.Roll, Is.EquivalentTo(_roll));
        }

        public SearchResultSelectionVerification Results(params string[] expected)
        {
            _results = expected;
            return this;
        }

        public SearchResultSelectionVerification Roll(params int[] expected)
        {
            _roll = expected;
            return this;
        }
    }
}
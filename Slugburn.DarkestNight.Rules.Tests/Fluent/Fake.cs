using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public static class Fake
    {
        public static IFakeContext Rolls(params int[] upcomingRolls)
        {
            var die = (FakeDie) Die.Implementation;
            die.AddUpcomingRolls(upcomingRolls);
            return new FakeContext();
        }

        private class FakeContext : IFakeContext
        {
        }
    }
}
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Tests.Fakes
{
    public static class FakeRollExtension
    {
        public static T Rolls<T>(this T context, params int[] upcomingRolls) where T : IFakeRollContext
        {
            var die = (FakeDie) Die.Implementation;
            die.AddUpcomingRolls(upcomingRolls);
            return context;
        } 
    }
}
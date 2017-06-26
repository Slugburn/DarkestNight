using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public static class ActionExtension
    {
        public static IPlayerActionContext Fights(this IPlayerActionContext context, string targetName, IFakeContext fake = null)
        {
            return context.CompletesConflict("Fight", targetName, fake);
        }

        public static IPlayerActionContext Eludes(this IPlayerActionContext context, string targetName, IFakeContext fake = null)
        {
            return context.CompletesConflict("Elude", targetName, fake);
        }

        public static IPlayerActionContext CompletesConflict(this IPlayerActionContext context, string tacticName, string targetName, IFakeContext fake = null)
        {
            return context
                .Targets(targetName)
                .UsesTactic(tacticName)
                .ResolvesConflict(fake)
                .AcceptsRoll()
                .AcceptsConflictResults();
        }
    }
}

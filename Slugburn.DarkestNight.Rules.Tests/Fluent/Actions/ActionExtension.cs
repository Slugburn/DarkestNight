using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public static class ActionExtension
    {
        public static IPlayerActionContext Fights(this IPlayerActionContext context, IFakeContext fake = null)
        {
            return context.CompletesConflict((string) null, "Fight", fake);
        }

        public static IPlayerActionContext Fights(this IPlayerActionContext context, string targetName, IFakeContext fake = null)
        {
            return context.CompletesConflict(targetName, "Fight", fake);
        }

        public static IPlayerActionContext Eludes(this IPlayerActionContext context, IFakeContext fake = null)
        {
            return context.CompletesConflict((string) null, "Elude", fake);
        }
        public static IPlayerActionContext Eludes(this IPlayerActionContext context, string targetName, IFakeContext fake = null)
        {
            return context.CompletesConflict(targetName, "Elude", fake);
        }

        public static IPlayerActionContext CompletesConflict(this IPlayerActionContext context, string targetName, string tacticName, IFakeContext fake = null)
        {
            return context.CompletesConflict(new [] {targetName}, tacticName, fake);
        }

        public static IPlayerActionContext CompletesConflict(this IPlayerActionContext context, string[] targetNames, string tacticName, IFakeContext fake = null)
        {
            return context
                .Targets(targetNames)
                .UsesTactic(tacticName)
                .ResolvesConflict(fake)
                .AcceptsRoll()
                .AcceptsConflictResults();
        }

        public static IPlayerActionContext StartsTurn(this IPlayerActionContext context, string heroName = null)
        {
            return context
                .Given.Game.NewDay()
                .When.Player.TakesAction(null, "Start Turn");
        }

        public static IPlayerActionContext CompletesSearch(this IPlayerActionContext context)
        {
            return context.Player.TakesAction("Search").AcceptsRoll().SelectsSearchResult();
        }
    }
}

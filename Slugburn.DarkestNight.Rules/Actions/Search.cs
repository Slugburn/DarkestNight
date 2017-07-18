using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Search : StandardAction
    {
        public Search() : base("Search")
        {
            Text = "Roll 1d and compare to the search difficulty listed on the board for your location.\n"
                   + "If your result equals or exceeds the difficulty, draw a search result for your location.\n"
                   + "If you roll multiple dice (e.g., using a power), draw one card for each die result that is greater than or equal to\n"
                   + "the search difficulty, then choose one card to resolve and ignore the others.";
        }

        public override bool IsAvailable(Hero hero)
        {
            return base.IsAvailable(hero) && hero.Location != Location.Monastery;
        }

        public override void Execute(Hero hero)
        {
            DoSearch(hero, 1);
        }

        internal static async void DoSearch(Hero hero, int diceCount)
        {
            hero.State = HeroState.Searching;
            var space = hero.Space;
            var state = hero.SetRoll(RollBuilder.Create<SearchRollHandler>()
                .Type(ModifierType.SearchDice)
                .Base("Search", diceCount)
                .Target(space.GetSearchTarget(hero) ?? 0));
            await hero.Triggers.Send(HeroTrigger.Searched);
            state.Roll();
            hero.DisplaySearch(CallbackHandler);
        }

        public class SearchRollHandler : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                hero.DisplaySearch(CallbackHandler);
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                if (rollState.Win)
                    hero.DrawSearchResults(rollState.Successes);
                else
                    hero.ContinueTurn();
            }
        }

        public static ICallbackHandler<Find> CallbackHandler => new MyCallbackHandler();

        private class MyCallbackHandler : ICallbackHandler<Find>
        {
            public void HandleCallback(Hero hero, Find data)
            {
                hero.ContinueTurn();
            }
        }
    }
}

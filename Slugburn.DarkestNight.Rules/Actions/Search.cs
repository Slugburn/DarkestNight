using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Search : IAction
    {
        public string Name => "Search";

        public string Text => "Roll 1d and compare to the search difficulty listed on the board for your location.\n"
                              + "If your result equals or exceeds the difficulty, draw a search result for your location.\n"
                              + "If you roll multiple dice (e.g., using a power), draw one card for each die result that is greater than or equal to\n"
                              + "the search difficulty, then choose one card to resolve and ignore the others.";

        public bool IsAvailable(Hero hero)
        {
            return hero.IsTakingTurn && hero.IsActionAvailable && hero.Location != Location.Monastery;
        }

        public void Act(Hero hero)
        {
            var space = hero.GetSpace();
            var state = hero.SetRoll(RollBuilder.Create<SearchRollHandler>()
                .Type(RollType.Search)
                .Base("Search", 1)
                .Target(space.SearchTarget));
            hero.Triggers.Send(HeroTrigger.Searched);
            state.Roll();
            hero.IsActionAvailable = false;
        }

        public class SearchRollHandler : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                if (rollState.Win)
                {
                    new DrawSearchCommand().DrawSearchResult(hero);
                }
                else
                {
                }
            }
        }

    }
}

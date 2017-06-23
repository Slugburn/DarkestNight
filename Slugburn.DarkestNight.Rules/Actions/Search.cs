using System;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Search : IAction
    {
        public string Name => "Search";
        public void Act(Hero hero)
        {
            hero.Triggers.Send(HeroTrigger.Searching);
            var space = hero.GetSpace();
            var state = hero.SetRoll(RollBuilder.Create<SearchRollHandler>()
                .Type(RollType.Search)
                .Base("Search", 1)
                .Target(space.SearchTarget));
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
                    hero.DrawSearchResult();
                }
                else
                {
                }
            }
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsActionAvailable && hero.Location != Location.Monastery;
        }
    }
}

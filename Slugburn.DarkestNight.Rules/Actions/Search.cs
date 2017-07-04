﻿using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Search : StandardAction, ICallbackHandler
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
            var space = hero.GetSpace();
            var state = hero.SetRoll(RollBuilder.Create<SearchRollHandler>()
                .Type(ModifierType.SearchDice)
                .Base("Search", 1)
                .Target(space.GetSearchTarget(hero)));
            hero.Triggers.Send(HeroTrigger.Searched);
            state.Roll();
            hero.Player.DisplaySearch(SearchModel.From(hero, null), Callback.ForCommand(hero, this));
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
                    hero.DrawSearchResults(rollState.Successes);
                else
                    hero.Player.DisplaySearch(null, null);
            }
        }

        public void HandleCallback(Hero hero, string path, object data)
        {
            hero.Player.DisplaySearch(null, null);
            hero.ContinueTurn();
        }
    }
}

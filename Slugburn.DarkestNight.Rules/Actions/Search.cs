using System;
using System.Linq;
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
            hero.SetRollHandler(new SearchRollHandler());
            var dice = hero.GetSearchDice();
            hero.Roll = Die.Roll(dice.Total).ToList();
            hero.Triggers.Send(HeroTrigger.AfterRoll);
            hero.State = HeroState.RollAvailable;
            hero.IsActionAvailable = false;
        }

        public class SearchRollHandler : IRollHandler
        {
            public void HandleRoll(Hero hero)
            {
                throw new NotImplementedException();
            }
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsActionAvailable && hero.Location != Location.Monastery;
        }

        public void HandleRoll(Hero hero)
        {
            throw new NotImplementedException();
        }
    }
}

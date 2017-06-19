using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Search : IAction
    {
        public string Name => "Search";
        public void Act(Hero hero)
        {
            hero.SetRollHandler(new SearchRollHandler());
            var dice = hero.GetSearchDice();
            hero.Roll = hero.Player.RollDice(dice.Total).ToList();
            hero.State = HeroState.RollAvailable;
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

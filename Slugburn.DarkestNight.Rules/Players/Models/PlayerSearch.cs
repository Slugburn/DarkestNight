using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerSearch
    {
        public List<string> SearchResults { get; set; }
        public List<int> Roll { get; set; }

        public static PlayerSearch From(Hero hero, IEnumerable<Find> searchResults)
        {
            var search = new PlayerSearch {Roll = hero.CurrentRoll.AdjustedRoll};
            if (searchResults!= null)
                search.SearchResults = searchResults.Select(x => x.ToDescription()).ToList();
            return search;
        }
    }
}

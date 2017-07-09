using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class SearchModel
    {
        public List<SearchResultModel> SearchResults { get; set; }
        public List<int> Roll { get; set; }

        public static SearchModel Create(Hero hero, IEnumerable<Find> finds)
        {
            var search = new SearchModel {Roll = hero.CurrentRoll.AdjustedRoll};
            if (finds!= null)
                search.SearchResults = SearchResultModel.Create(finds);
            return search;
        }
    }

    public class SearchResultModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }

        public static SearchResultModel Create(Find find)
        {
            return new SearchResultModel
            {
                Code = find.ToString(),
                Name = find.ToDescription(),
            };
        }

        public static List<SearchResultModel> Create(IEnumerable<Find> finds)
        {
            return finds.Select(Create).ToList();
        }
    }
}

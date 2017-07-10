using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class SearchResultVm
    {

        public static List<SearchResultVm> Create(IEnumerable<SearchResultModel> models)
        {
            return models?.Select(Create).ToList();
        }

        public static SearchResultVm Create(SearchResultModel model)
        {
            return new SearchResultVm
            {
                Code = model.Code,
                Name = model.Name,
                Text = model.Text
            };
        }

        public Find Code { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
    }
}
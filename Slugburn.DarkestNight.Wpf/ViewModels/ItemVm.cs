using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class ItemVm
    {
        public ItemVm(ItemModel model)
        {
            Name = model.Name;
            Text = model.Text;
        }

        public string Name { get; set; }
        public string Text { get; set; }

        public static List<ItemVm> Create(IEnumerable<ItemModel> models)
        {
            return models.Select(model => new ItemVm(model)).ToList();
        }
    }
}

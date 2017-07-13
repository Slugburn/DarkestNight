using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class ItemVm
    {
        public ItemVm(ItemModel model)
        {
            Id = model.Id;
            Name = model.Name;
            Text = model.Text;
            Owner = model.Owner;
        }

        public ItemVm()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Owner { get; set; }

        public static List<ItemVm> Create(IEnumerable<ItemModel> models)
        {
            return models.Select(model => new ItemVm(model)).ToList();
        }
    }
}

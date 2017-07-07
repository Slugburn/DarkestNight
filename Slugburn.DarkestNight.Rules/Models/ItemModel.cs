using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Items;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class ItemModel
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public static ItemModel Create(IItem item)
        {
            return new ItemModel {Name = item.Name, Text = item.Text};
        }

        public static List<ItemModel> Create(IEnumerable<IItem> inventory)
        {
            return inventory.Select(Create).ToList();
        }
    }
}
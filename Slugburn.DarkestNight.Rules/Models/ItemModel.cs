using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Items;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Owner { get; set; }

        public static ItemModel Create(IItem item)
        {
            return new ItemModel {Id = item.Id, Name = item.Name, Text = item.Text, Owner = item.Owner.Name};
        }


        public static List<ItemModel> Create(IEnumerable<IItem> inventory)
        {
            return inventory.Select(Create).ToList();
        }
    }
}
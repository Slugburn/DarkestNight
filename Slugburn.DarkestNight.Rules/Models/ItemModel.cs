using Slugburn.DarkestNight.Rules.Items;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class ItemModel
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public static ItemModel FromItem(IItem item)
        {
            return new ItemModel() {Name = item.Name, Text = item.Text};
        }
    }
}
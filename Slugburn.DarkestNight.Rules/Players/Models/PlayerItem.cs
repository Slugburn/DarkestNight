using Slugburn.DarkestNight.Rules.Items;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerItem
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public static PlayerItem FromItem(IItem item)
        {
            return new PlayerItem() {Name = item.Name, Text = item.Text};
        }
    }
}
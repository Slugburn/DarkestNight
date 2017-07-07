using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class HeroActionModel
    {
        public HeroActionModel(Hero hero)
        {
            HeroName = hero.Name;
            Commands = CommandModel.Create(hero.AvailableCommands);
            Powers = PowerModel.Create(hero.Powers);
            Items = ItemModel.Create(hero.GetInventory());
        }

        public string HeroName { get; set; }
        public IEnumerable<CommandModel> Commands { get; set; }
        public IEnumerable<PowerModel> Powers { get; set; }
        public IEnumerable<ItemModel> Items { get; set; }
    }
}

using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class HeroModel
    {
        public string Name { get; set; }
        public HeroStatusModel Status { get; set; }
        public List<CommandModel> Commands { get; set; }
        public List<ItemModel> Inventory { get; set; }


        public static HeroModel FromHero(Hero hero)
        {
            return new HeroModel
            {
                Name = hero.Name,
                Status = HeroStatusModel.FromHero(hero),
                Commands = hero.AvailableCommands?.Select(CommandModel.FromCommand).ToList(),
                Inventory = hero.GetInventory().Select(ItemModel.FromItem).ToList()
            };
        }
    }
}
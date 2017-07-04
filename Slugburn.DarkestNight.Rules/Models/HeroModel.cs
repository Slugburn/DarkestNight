using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class HeroModel
    {
        public string Name { get; set; }
        public HeroStatusModel Status { get; set; }
        public List<PowerModel> Powers { get; set; }
        public List<CommandModel> Commands { get; set; }
        public List<ItemModel> Inventory { get; set; }


        public static HeroModel Create(Hero hero)
        {
            return new HeroModel
            {
                Name = hero.Name,
                Status = HeroStatusModel.FromHero(hero),
                Powers = PowerModel.Create(hero.Powers),
                Commands = CommandModel.Create(hero.AvailableCommands),
                Inventory = hero.GetInventory().Select(ItemModel.FromItem).ToList()
            };
        }
    }
}
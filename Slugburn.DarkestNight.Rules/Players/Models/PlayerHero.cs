using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerHero
    {
        public string Name { get; set; }
        public PlayerHeroStatus Status { get; set; }
        public List<PlayerCommand> Commands { get; set; }
        public List<PlayerItem> Inventory { get; set; }


        public static PlayerHero FromHero(Hero hero)
        {
            return new PlayerHero
            {
                Name = hero.Name,
                Status = PlayerHeroStatus.FromHero(hero),
                Commands = hero.AvailableCommands?.Select(PlayerCommand.FromCommand).ToList(),
                Inventory = hero.GetInventory().Select(PlayerItem.FromItem).ToList()
            };
        }
    }
}
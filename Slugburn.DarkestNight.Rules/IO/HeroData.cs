using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Items;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.IO
{
    public class HeroData
    {
        public string Name { get; set; }
        public int DefaultGrace { get; set; }
        public int DefaultSecrecy { get; set; }
        public int Grace { get; set; }
        public int Secrecy { get; set; }
        public Location Location { get; set; }
        public List<string> PowerDeck { get; set; }
        public List<PowerData> Powers { get; set; }
        public List<string> Inventory { get; set; }

        public void Restore(Game game)
        {
            var hero = new Hero
            {
                Name = Name,
                DefaultGrace = DefaultGrace,
                DefaultSecrecy = DefaultSecrecy,
                Grace = Grace,
                Secrecy = Secrecy,
                Location = Location,
            };
            hero.PowerDeck.AddRange(PowerDeck);
            game.AddHero(hero, game.Players.Single());
            foreach (var powerData in Powers)
            {
                var power = PowerFactory.Create(powerData.Name);
                hero.LearnPower(power);
                if (powerData.Exhausted)
                    power.Exhaust(hero);
                if (powerData.IsActive ?? false)
                {
                    var activatable = power as IActivateable;
                    activatable?.Activate(hero);
                }
            }
            foreach (var itemName in Inventory)
                hero.AddToInventory(game.CreateItem(itemName));
        }
    }
}
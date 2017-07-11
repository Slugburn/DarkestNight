using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.IO
{
    public class HeroData
    {
        public string Name { get; set; }
        public int Grace { get; set; }
        public int Secrecy { get; set; }
        public Location Location { get; set; }
        public List<string> PowerDeck { get; set; }
        public List<PowerData> Powers { get; set; }
        public List<string> Inventory { get; set; }

        public void Restore(Game game)
        {
            var hero = HeroFactory.Create(Name);
            hero.Grace = Grace;
            hero.Secrecy = Secrecy;
            hero.Location = Location;
            hero.PowerDeck.Clear();
            hero.PowerDeck.AddRange(PowerDeck);
            game.AddHero(hero, game.Players.Single());
            foreach (var powerData in Powers)
                powerData.Restore(hero);
            foreach (var itemName in Inventory)
                hero.AddToInventory(game.CreateItem(itemName));
        }
    }
}